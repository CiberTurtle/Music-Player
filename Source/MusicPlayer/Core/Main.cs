using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using MusicPlayer.UI;
using MusicPlayer.UI.Menus;

namespace MusicPlayer
{
	public class Main : Game
	{
		public static Main current { get; private set; }

		public static GraphicsDeviceManager graphics;
		public static SpriteBatch sb;

		static Random _rng;
		public static Random rng
		{
			get
			{
				if (_rng == null)
					_rng = new Random(DateTime.Now.Ticks.GetHashCode());
				return new Random(DateTime.Now.Ticks.GetHashCode());
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
						_settings = JsonConvert.DeserializeObject<Settings>(json);

					WriteSettings();
				}

				return _settings;
			}
		}

		public static string root;
		public static string settingsPath = @"\settings.json";

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
			current = this;

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			IsFixedTimeStep = true;

			graphics.SynchronizeWithVerticalRetrace = true;
			graphics.PreferMultiSampling = true;
			graphics.ApplyChanges();

			Window.TextInput += (sender, args) => Input.TextInput(args);

			root = Environment.CurrentDirectory;
			settingsPath = root + settingsPath;

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { Formatting = Formatting.Indented };

			File.WriteAllText(root + "/settings.schema.json", new JSchemaGenerator().Generate(typeof(Settings)).ToString(SchemaVersion.Draft7));

			volume = settings.startingVolume;

			Menu.OpenMenu(new MenuMain());

			ReloadData();
		}

		protected override void Initialize()
		{
			CreateFilesAndFolders();
			OutputSys.UpdateAllOutputs(true);

			base.Initialize();
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			ReloadData();
			OutputSys.UpdateAllOutputs(true);

			// WriteSettings();

			base.OnExiting(sender, args);
		}

		protected override void LoadContent()
		{
			sb = new SpriteBatch(GraphicsDevice);

			font = Content.Load<SpriteFont>("Fonts/Regular");
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
					MusicSys.PlayRandomSongFromPlaylist();

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

			Menu.Update();

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
			GraphicsDevice.Clear(new Color("#1110"));

			sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

			GUI.Draw();

			sb.End();

			base.Draw(gameTime);
		}

		public static void ReloadData()
		{
			_settings = null;
			MusicSys.playlists = null;
			current.InactiveSleepTime =
				settings.throttleWhenUnfocused ? new TimeSpan((long)(0.1f * TimeSpan.TicksPerSecond)) : TimeSpan.Zero;

			Main.CreateFilesAndFolders();
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