extends Node2D

@export var target: CharacterBody2D = null

var _physics_delta: float = 0.0

func _process(_delta: float) -> void:
	var motion: Vector2 = target.velocity * _physics_delta
	var fraction: float = Engine.get_physics_interpolation_fraction()
	
	position = lerp(Vector2.ZERO, motion, fraction)

func _physics_process(delta: float) -> void:
	_physics_delta = delta
