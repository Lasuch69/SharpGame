extends Control

func _on_stats_button_pressed() -> void:
	print("Nothing to see here yet.")

func _on_menu_button_pressed() -> void:
	get_tree().change_scene_to_file("res://scenes/main.tscn")
	pass
