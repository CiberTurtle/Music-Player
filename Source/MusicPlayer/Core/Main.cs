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
using System.Linq;

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
				return _rng;
			}
		}

		public static string root;

		public static bool enableAutoplay = true;
		public static bool enableOutput = true;

		public static double tickCooldown;
		public static long lastTickTime;
		public static TimeSpan timePlayed;
		public static double deltaTime;

		public static List<string> errors = new List<string>();

		// Content
		public static SpriteFont font;

		public Main()
		{
			current = this;

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			IsFixedTimeStep = false;

			graphics.SynchronizeWithVerticalRetrace = true;
			graphics.PreferMultiSampling = true;
			graphics.ApplyChanges();

			Window.TextInput += (sender, args) => Input.TextInput(args);

			root = Environment.CurrentDirectory;

			var args = Environment.GetCommandLineArgs().ToList();

			if (args.Contains("--clear-settings"))
				File.WriteAllText(Settings.settingsPath, string.Empty);

			if (args.Contains("--clear-output"))
				Directory.Delete(Settings.current.outputFolder);

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { Formatting = Formatting.Indented };

			File.WriteAllText(root + "/settings.schema.json", new JSchemaGenerator().Generate(typeof(Settings)).ToString(SchemaVersion.Draft7));

			MusicSys.volume = Settings.current.startingVolume;

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
			deltaTime = delta.TotalSeconds;

			lastTickTime = DateTime.Now.Ticks;
			Input.Update();

			if (IsActive)
			{
				if (Input.CheckButtonPress(Settings.current.volumeUpKey)) MusicSys.volume++;
				if (Input.CheckButtonPress(Settings.current.volumeDownKey)) MusicSys.volume--;
				if (Input.CheckButtonPress(Settings.current.pauseKey)) MusicSys.TogglePause();
			}

			if (MusicSys.currentSongInstance != null)
			{
				switch (MusicSys.currentSongInstance.State)
				{
					case SoundState.Playing:
						timePlayed += delta;
						break;
					case SoundState.Paused:
						break;
					case SoundState.Stopped:
						if (enableAutoplay) MusicSys.PlayRandomSongFromPlaylist();
						break;
				}
			}

			if (tickCooldown < 0)
			{
				tickCooldown = Settings.current.tickTime;
				OutputSys.UpdateOutput(true);
			}
			tickCooldown -= deltaTime;

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
			Settings.MakeDataNull();
			MusicSys.playlists = null;
			current.InactiveSleepTime =
				Settings.current.throttleWhenUnfocused ? new TimeSpan((long)(0.1f * TimeSpan.TicksPerSecond)) : TimeSpan.Zero;

			Main.CreateFilesAndFolders();
		}

		public static void CreateFilesAndFolders()
		{
			if (!Directory.Exists(Settings.current.musicFolder))
				Directory.CreateDirectory(Settings.current.musicFolder);

			if (!Directory.Exists(Settings.current.outputFolder))
				Directory.CreateDirectory(Settings.current.outputFolder);
		}

		public static void LogError(string error)
		{
			errors.Add(error);
		}
	}
}