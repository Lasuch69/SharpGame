extends Node

var _projectile_component: ProjectileComponent = null

func _spawn_projectile(p_position: Vector2, p_velocity: Vector2, p_collision_mask: int) -> void:
	ProjectileSystem.spawn_projectile(p_position, p_velocity, p_collision_mask, _on_collision)

func _on_collision(p_position: Vector2, p_result: Dictionary) -> void:
	_projectile_component.OnCollisionCallback(p_position, p_result)
