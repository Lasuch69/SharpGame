extends Control

signal load_level(path: String)

func _on_play_button_pressed() -> void:
	load_level.emit("res://levels/level_1.tscn")

func _on_exit_button_pressed() -> void:
	get_tree().quit()
