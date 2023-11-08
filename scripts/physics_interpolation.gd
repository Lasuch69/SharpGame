extends Node2D

@export
var character_body: CharacterBody2D

func _process(_delta: float) -> void:
	var motion: Vector2 = character_body.velocity * get_physics_process_delta_time()
	var fraction: float = Engine.get_physics_interpolation_fraction()
	position = lerp(Vector2.ZERO, motion, fraction)
