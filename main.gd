extends Node

enum GameState { MENU, LOADING, GAME, PAUSE }

var current_game_state := GameState.MENU

@onready
var _loader: ThreadedLoader = $ThreadedLoader

func _ready() -> void:
	%Menu.load_level.connect(go_to_level)

func go_to_menu() -> void:
	var children = %GameWorld.get_children()
	
	for child in children:
		%GameWorld.remove_child(child)
	
	%Menu.visible = true
	current_game_state = GameState.MENU

func go_to_level(path: String) -> void:
	var handle = _loader.request_resource(path)
	current_game_state = GameState.LOADING
	%Menu.visible = false
	
	handle.resource_loaded.connect(_on_level_loaded)

func _on_level_loaded(_path: String, scene: PackedScene) -> void:
	%GameWorld.add_child(scene.instantiate())
	current_game_state = GameState.GAME
