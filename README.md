# Music Player (indev)

## Instructions

### Quick Setup

1. Add a playlist following the folder layout from #Folder Layout
2. Run the program using `dotnet run` or `F5` if you're in vscode
3. Hit `F5` to play a random song
4. Add a text source to obs
5. Set the text source to read from a text file from the `Output` folder
6. **Profit!!!**

### Folder Layout

```
- Music
	- Playlist
		- A song that will play.wav
		- A person who makes music_A song that was made by a person who makes music.wav
		- @ A song that won't play.wav
		- @A song that also won't play.wav
		- A song that can't play.mp3
	- @ Disabled Playlist

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
	"$schema": "./settings_schema.json", // Ignore this
	"startingVolume": 10, // The starting volume in increments
	"volumeIncrements": 10, // The max volume
	"tickTime": 1.0, // Time between ticks
	"settingsKey": "F1",
	"toggleOutputKey": "F2",
	"reloadKey": "F5",
	"volumeUpKey": "OemPlus",
	"volumeDownKey": "OemMinus",
	"pauseKey": "Space",
	"musicPath": "/Music", // The relitive to a collection of playlists
	"outputPath": "/Output", // The relitive to where all the text files will be outputed to
	"windowTexts": [
		// The text that will be in the window (see #Parameters) (use "#" to make the line bold)
		"#{song_artist}: {song_name} from {playlist_name}",
		"{song_timeplayed_mm:ss.ff} {song_timebar_10} {song_duration_mm:ss.ff}",
		"{audio_volume} / {audio_volume_max} ('{input_volume_down}' '{input_volume_up}')",
		"Outputting {output_enabled} ('{input_output}')",
		"Settings ('{input_settings}')",
		"Reload ('{input_reload}')"
	],
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
			"text": "{song_timeplayed_mm:ss} / {song_duration_mm:ss}"
		}
	]
}
```

### Parameters

Format: `{parameter_name}[argument_#1, argument_#2, argument_#3]`

| Parameter Name             | Description                                                |
| -------------------------- | ---------------------------------------------------------- |
| `song_name`                | The song's name (never could have guessed)                 |
| `song_artist`              | The person of the song/origin of the song                  |
| `playlist_name`            | The name of the active playlist                            |
| `song_duration_mm:ss`      | The length of the song in `minutes:seconds`                |
| `song_duration_mm:ss.ff`   | The length of the song in `minutes:seconds.milliseconds`   |
| `song_timeplayed_mm:ss`    | The playtime of the song in `minutes:seconds`              |
| `song_timeplayed_mm:ss.ff` | The playtime of the song in `minutes:seconds.milliseconds` |
| `song_timebar_10`          | A nice looking bar that is 10 segments long `[----------]` |
| `audio_volume`             | The current volume in increments                           |
| `audio_volume_max`         | The max volume in increments                               |
| `audio_volume_percent`     | The volume as a percent rounded to the nearest hole number |
| `output_enabled`           | Whether outputing is enabled                               |
| `input_settings`           | The key to open settings.json                              |
| `input_reload`             | The key to reaload                                         |
| `input_volume_up`          | The key to raise the volume                                |
| `input_volume_up`          | The key to lower the volume                                |

## Plans

- Better code
  - Better optimisation
  - Better organization
- Better parameters
  - Parameter arguments
  - Better parsing
- Better Music
  - Support for `.mp3` files
  - Music streaming
  - Music from the web?
- An actual GUI
  - That would be neat
