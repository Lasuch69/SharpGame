extends Node

const PATH: String = "user://settings.json"
const VERSION: String = "0.0.0"

func save_settings():
	var settings: Dictionary = {}
	
	settings["version"] = VERSION
	
	var bus_count: int = AudioServer.bus_count
	var audio: Dictionary = {}

	for i in bus_count:
		var busName: String = AudioServer.get_bus_name(i)
		var volume: float = AudioServer.get_bus_volume_db(i)
		audio[busName.to_snake_case()] = db_to_linear(volume)
	
	settings["audio"] = audio

	var windowMode: int = DisplayServer.window_get_mode()
	settings["window_mode"] = windowMode

	var json_string: String = JSON.stringify(settings, "\t", false)
	
	var file: FileAccess = FileAccess.open(PATH, FileAccess.WRITE)
	
	file.store_string(json_string)
	
	file = null

func load_settings():
	var file: FileAccess = FileAccess.open(PATH, FileAccess.READ);
	
	var json_string: String = file.get_as_text()

	var settings: Dictionary = JSON.parse_string(json_string)
	
	var audio: Dictionary = settings.get("audio", {});
	
	for key in audio.keys():
		var volume: float = audio.get(key, 1.0)
		var bus_index: int = AudioServer.get_bus_index(key.to_pascal_case())
		AudioServer.set_bus_volume_db(bus_index, linear_to_db(volume))
	
	var window_mode: int = settings.get("window_mode", DisplayServer.WINDOW_MODE_FULLSCREEN)
	DisplayServer.window_set_mode(window_mode)
	
	file = null
