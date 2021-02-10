using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.Diagnostics;

namespace MusicPlayer
{
	public class Main : Game
	{
		public static GraphicsDeviceManager graphics;
		public static SpriteBatch sb;

		static Random _rng;
		public static Random rng
		{
			get
			{
				if (_rng == null)
					_rng = new Random(DateTime.Now.Millisecond);
				return _rng;
			}
		}

		static JsonSerializer _serializer;
		public static JsonSerializer serializer
		{
			get
			{
				if (_serializer == null)
				{
					_serializer = JsonSerializer.Create(new JsonSerializerSettings()
					{

					});
				}

				return _serializer;
			}
		}

		static Settings _settings;
		public static Settings settings
		{
			get
			{
				if (_settings == null)
				{
					if (!File.Exists(settingsPath))
						File.Create(settingsPath);

					var json = File.ReadAllText(settingsPath);

					if (string.IsNullOrEmpty(json))
						_settings = new Settings();
					else
					{
						_settings = JsonConvert.DeserializeObject<Settings>(json);
					}

					WriteSettings();
				}

				return _settings;
			}
		}

		public static string path;
		public static string settingsPath = @"\settings.json";

		public static Point kirbySize = new Point(64 * 4);
		public static Texture2D texKirby;
		public static SpriteFont bold;
		public static SpriteFont font;

		static int _volume;
		public static int volume
		{
			get => _volume;
			set
			{
				value = Math.Clamp(value, 0, settings.volumeIncrements);

				_volume = value;
				SoundEffect.MasterVolume = value * (1.0f / settings.volumeIncrements);
			}
		}
		public static bool enableOutput = true;

		public static double tickCooldown;
		public static long lastTickTime;
		public static TimeSpan timePlayed;

		public Main()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			IsFixedTimeStep = true;

			graphics.SynchronizeWithVerticalRetrace = true;
			graphics.ApplyChanges();

			path = Environment.CurrentDirectory;
			settingsPath = path + settingsPath;

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { Formatting = Formatting.Indented };

			File.WriteAllText(path + "/schema.json", new JSchemaGenerator().Generate(typeof(Settings)).ToString(SchemaVersion.Draft7));

			InactiveSleepTime = new TimeSpan(666666 * 2);

			volume = settings.startingVolume;

			Window.TextInput += (sender, args) => Input.TextInput(args);
		}

		protected override void Initialize()
		{
			OutputSys.UpdateAllOutputs(true);

			base.Initialize();
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			NullAllData();
			OutputSys.UpdateAllOutputs(true);

			// WriteSettings();

			base.OnExiting(sender, args);
		}

		protected override void LoadContent()
		{
			sb = new SpriteBatch(GraphicsDevice);

			texKirby = Content.Load<Texture2D>("Sprites/Kirby");
			font = Content.Load<SpriteFont>("Fonts/Regular");
			bold = Content.Load<SpriteFont>("Fonts/Bold");
		}

		protected override void Update(GameTime gameTime)
		{
			var delta = new TimeSpan(DateTime.Now.Ticks - lastTickTime);
			var deltaTime = delta.TotalSeconds;

			lastTickTime = DateTime.Now.Ticks;
			Input.Update();

			if (IsActive)
			{
				if (Input.CheckButtonPress(settings.reloadKey))
				{
					NullAllData();
					MusicSys.PlayRandomSong();
				}

				if (Input.CheckButtonPress(settings.settingsKey))
					Process.Start(new ProcessStartInfo("explorer", "\"" + settingsPath + "\""));

				if (Input.CheckButtonPress(settings.volumeUpKey)) volume++;
				if (Input.CheckButtonPress(settings.volumeDownKey)) volume--;

				if (Input.CheckButtonPress(settings.toggleOutputKey)) enableOutput = !enableOutput;

				// if(Input.CheckButtonPress(settings.pauseKey)) ;
			}

			if (MusicSys.currentSongInstance != null)
			{
				if (MusicSys.currentSongInstance.State != SoundState.Playing)
					MusicSys.PlayRandomSong();

				timePlayed += delta;
			}

			if (tickCooldown < 0)
			{
				tickCooldown = settings.tickTime;
				OutputSys.UpdateOutput(true);
			}
			tickCooldown -= deltaTime;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Alpha zero so you can setup a game capture in obs
			GraphicsDevice.Clear(new Color(17, 17, 17, 0));

			using (var layout = new StackLayout(new Vector2(8), 32, false))
			{
				sb.Draw(
					texKirby,
					new Rectangle(
						graphics.PreferredBackBufferWidth / 2 + (int)kirbySize.X,
						graphics.PreferredBackBufferHeight / 2 + (int)kirbySize.Y,
						kirbySize.X,
						kirbySize.Y),
					Color.White);

				foreach (var line in settings.windowTexts)
				{
					if (line.StartsWith('#'))
						sb.DrawString(bold, OutputSys.ParsePerams(line.Remove(0, 1)), layout.AddElement(), Color.White);
					else
						sb.DrawString(font, OutputSys.ParsePerams(line), layout.AddElement(), Color.White);
				}
			}

			base.Draw(gameTime);
		}

		public static void NullAllData()
		{
			_settings = null;
			MusicSys.currentSong?.Dispose();
			MusicSys.currentSong = null;
			MusicSys.currentSongInstance?.Dispose();
			MusicSys.currentSongInstance = null;
			MusicSys.currentPlaylistPath = string.Empty;
		}

		public static void WriteSettings()
		{
			// Kinda hacky but idk
			File.WriteAllText(settingsPath, "{\n\t\"$schema\": \"./schema.json\"," + JsonConvert.SerializeObject(_settings).Remove(0, 1));
		}
	}
}