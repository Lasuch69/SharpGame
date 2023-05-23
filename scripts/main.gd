extends Control

const world = preload("res://scenes/world.tscn")

func _on_play_button_pressed():
	get_tree().change_scene_to_packed(world)

func _on_exit_button_pressed():
	get_tree().quit()
