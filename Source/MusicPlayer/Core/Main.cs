using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using MusicPlayer.UI;

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

		public static string root;
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

		public static List<string> errors = new List<string>();

		public Main()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			IsFixedTimeStep = true;

			graphics.SynchronizeWithVerticalRetrace = true;
			graphics.ApplyChanges();

			root = Environment.CurrentDirectory;
			settingsPath = root + settingsPath;

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { Formatting = Formatting.Indented };

			File.WriteAllText(root + "/settings.schema.json", new JSchemaGenerator().Generate(typeof(Settings)).ToString(SchemaVersion.Draft7));

			InactiveSleepTime = new TimeSpan(666666);

			volume = settings.startingVolume;

			Window.TextInput += (sender, args) => Input.TextInput(args);
		}

		protected override void Initialize()
		{
			CreateFilesAndFolders();
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
				if (Input.CheckButtonPress(settings.volumeUpKey)) volume++;
				if (Input.CheckButtonPress(settings.volumeDownKey)) volume--;
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

			Pointer.Reset();
			GUI.Update();

			foreach (var text in settings.windowTexts)
				GUI.Text(OutputSys.ParsePerams(text));

			GUI.LineBreak();

			if (GUI.Button("Reload"))
			{
				NullAllData();
				CreateFilesAndFolders();
				MusicSys.PlayRandomSong();
			}

			if (GUI.Button("Outputting " + (enableOutput ? "ON" : "OFF")))
				enableOutput = !enableOutput;

			if (GUI.Button("Open Settings"))
				Process.Start(new ProcessStartInfo("explorer", "\"" + settingsPath + "\""));

			GUI.LineBreak();

			if (errors.Count > 0)
				GUI.Text($"Error ({errors.Count}): {errors[errors.Count - 1]}");
			else
				GUI.Text("No errors!");

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Alpha zero so you can setup a game capture in obs
			GraphicsDevice.Clear(Color.clear);

			sb.Begin();

			sb.Draw(
				texKirby,
				new Rectangle(
					graphics.PreferredBackBufferWidth / 2 + (int)kirbySize.X,
					graphics.PreferredBackBufferHeight / 2 + (int)kirbySize.Y,
					kirbySize.X,
					kirbySize.Y),
				Color.white);

			GUI.Draw();

			sb.End();

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
			File.WriteAllText(
				settingsPath,
				"{\n\t\"$schema\": \"./settings.schema.json\"," + JsonConvert.SerializeObject(_settings).Remove(0, 1)
			);
		}

		public static void CreateFilesAndFolders()
		{
			if (!Directory.Exists(Util.ParsePath(settings.musicPath)))
				Directory.CreateDirectory(Util.ParsePath(settings.musicPath));

			if (!Directory.Exists(Util.ParsePath(settings.outputPath)))
				Directory.CreateDirectory(Util.ParsePath(settings.outputPath));
		}

		public static void LogError(string error)
		{
			errors.Add(error);
		}
	}
}