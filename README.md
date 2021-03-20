# Music Player (indev)

## Instructions

### Quick Setup

1. Add a playlist following the folder layout from #Folder Layout
2. Run the program using `dotnet run` or `F5` if you're in vscode (or download it if there's one available)
3. Click play
4. Select/Add a playlist
5. Add a text source to obs
6. Set the text source to read from a text file from the `Output` folder
7. **Profit!!!**

### Folder Layout

```
- Music
	- Playlist
	- A song that will play.wav
		- A person who makes music_A song that was made by a person who makes music.wav
		- @ A song that won't play.wav
		- @A song that also won't play.wav
		- A song that can't play.mp3

- Output
	- song_artist.txt
	- song_name.txt
	- song_time.txt

	- settings.json (see #Settings)
	- settings_schema.json (Ignore this)
```

### Settings

```jsonc
{
	"$schema": "./settings.schema.json", // Ignore this
	"version": "1.0.2.0", // Ignore this too
	"startingVolume": 10, // The starting volume in increments
	"volumeIncrements": 10, // The max volume
	"tickTime": 1.0, // Time between ticks
	"throttleWhenUnfocused": true, // Caps fps when the windown is not active/focused
	"volumeUpKey": "OemPlus",
	"volumeDownKey": "OemMinus",
	"pauseKey": "Space",
	"staticUI": false, // Enable Animations
	"accentColor": { // Accent Color
		"hex": "#FF7504FF"
	},
	"musicPath": "/Music", // The path to a collection of playlists
	"outputPath": "/Output", // The path to where all the text files will be outputed to
	"disabledChar": "@", // The prefix character for anything that is disabled
	"windowTexts": [
		// The text that will be in the window (see #Parameters)
		"{song_artist}: {song_name} from {playlist_name}",
		"{song_time_mm:ss.ff} {song_time_bar_10} {song_duration_mm:ss.ff} {song_state_name}",
		"{audio_volume} {audio_volume_bar} {audio_volume_max}"
	],
	"windowTitle": "Music Player	[{song_time_mm:ss} / {song_duration_mm:ss}]	{song_artist} {song_name} from {playlist_name}", // The text that will be in the window's titlebar
	"outputsNewSong": [
		// Files that will be outputted to after a new song is played
		{
			"path": "/song_name.txt",
			"text": "{song_name}"
		},
		{
			"path": "/song_artist.txt",
			"text": "{song_artist}"
		}
	],
	"outputsEveryTick": [
		// Files that will be outputted at every tick (see "tickTime")
		{
			"path": "/song_time.txt",
						"text": "{song_time_mm:ss} / {song_duration_mm:ss}"
		}
	]
}
```

### Parameters

> Arguments are not supported yet

Format: `{parameter_name}[argument_#1, argument_#2, argument_#3]`

| Parameter Name             | Description                                                                                  |
| -------------------------- | -------------------------------------------------------------------------------------------- |
| `song_name`                | The song's name (never could have guessed)                                                   |
| `song_artist`              | The person of the song/origin of the song                                                    |
| `playlist_name`            | The name of the active playlist                                                              |
| `song_duration_mm:ss`      | The length of the song in `minutes:seconds`                                                  |
| `song_duration_mm:ss.ff`   | The length of the song in `minutes:seconds.milliseconds`                                     |
| `song_timeplayed_mm:ss`    | The playtime of the song in `minutes:seconds`                                                |
| `song_timeplayed_mm:ss.ff` | The playtime of the song in `minutes:seconds.milliseconds`                                   |
| `song_timebar_10`          | A nice looking bar that is 10 segments long to represent the time in the song `[----------]` |
| `audio_volume`             | The current volume in increments                                                             |
| `audio_volume_max`         | The max volume in increments                                                                 |
| `audio_volume_percent`     | The volume as a percent rounded to the nearest whole number                                  |
| `audio_volume_bar`         | The volume as a fairly neat looking bar with the amount of segments being the max volume     |
| `output_enabled`           | Whether outputing is enabled                                                                 |
| `input_volume_up`          | The key to raise the volume                                                                  |
| `input_volume_up`          | The key to lower the volume                                                                  |

## Plans

- Better code
	- Better optimisation
- Better parameters
	- Parameter arguments
	- Better parsing
- Better Music
	- Support for `.mp3` files
	- Music streaming
	- Music from the web?
- Better Other
	- Menu Scrolling
