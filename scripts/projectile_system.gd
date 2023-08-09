extends Node2D

var _physics_delta: float = 0.0
var _projectiles: Array[Projectile] = []
var _direct_space_state: PhysicsDirectSpaceState2D = null

func _physics_process(delta: float) -> void:
	if _direct_space_state == null:
		var space: RID = get_world_2d().space
		_direct_space_state = PhysicsServer2D.space_get_direct_state(space)
	
	_physics_delta = delta
	
	var to_remove: Array[Projectile] = []
	
	for i in _projectiles.size():
		var projectile: Projectile = _projectiles[i]
		
		var motion: Vector2 = projectile.velocity * delta
		var projectile_transform := Transform2D(0.0, projectile.position)
		var collision_mask: int = projectile.collision_mask
		
		var result: Dictionary = _check_collision(projectile_transform, motion, collision_mask)
		
		if !result.is_empty():
			var collision_point: Vector2 = projectile.position + motion
			projectile.callback.call(collision_point, result)
			to_remove.append(projectile)
			
			continue
		
		projectile.position += motion
	
	for projectile in to_remove:
		var index: int = _projectiles.find(projectile)
		_projectiles.remove_at(index)

func clear() -> void:
	_projectiles.clear()

func get_buffer(p_size: int) -> PackedFloat32Array:
	var fraction: float = Engine.get_physics_interpolation_fraction()
	var buffer: PackedFloat32Array = _get_buffer(_projectiles.duplicate(), _physics_delta, fraction)
	buffer.resize(p_size)
	
	return buffer

func get_projectile_count() -> int:
	return _projectiles.size()

func spawn_projectile(p_position: Vector2, p_velocity: Vector2, p_collision_mask: int, p_callback: Callable) -> void:
	var projectile := Projectile.new()
	projectile.position = p_position
	projectile.velocity = p_velocity
	
	projectile.collision_mask = p_collision_mask
	
	projectile.callback = p_callback
	
	_projectiles.append(projectile)

func _check_collision(p_transform: Transform2D, p_motion: Vector2, p_collision_mask: int) -> Dictionary:
	var shape := CircleShape2D.new()
	shape.radius = 3.0
	
	var parameters := PhysicsShapeQueryParameters2D.new()
	parameters.motion = p_motion
	parameters.transform = p_transform
	parameters.shape = shape
	parameters.collision_mask = p_collision_mask
	
	var result: Array[Dictionary] = _direct_space_state.intersect_shape(parameters, 1)
	
	return result[0] if !result.is_empty() else {}

static func _get_buffer(p_projectiles: Array[Projectile], p_delta: float, p_fraction: float) -> PackedFloat32Array:
	var buffer: PackedFloat32Array = []
	
	for projectile in p_projectiles:
		var movement: Vector2 = projectile.velocity * p_delta
		var out_position: Vector2 = lerp(projectile.position, projectile.position + movement, p_fraction)
		var angle: float = Vector2.ZERO.angle_to_point(projectile.velocity) + deg_to_rad(90)
		
		var projectile_transform := Transform2D(-angle, out_position)
		buffer.append_array(_to_float_array(projectile_transform))
	
	return buffer

static func _to_float_array(p_transform: Transform2D) -> PackedFloat32Array:
	var b_x: Vector2 = p_transform.x
	var b_y: Vector2 = p_transform.y
	var origin: Vector2 = p_transform.origin
	
	return [b_x.x, b_x.y, 0, origin.x, b_y.x, b_y.y, 0, origin.y]
