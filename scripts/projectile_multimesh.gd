class_name ProjectileMultiMesh
extends MultiMeshInstance2D

func _process(delta: float) -> void:
	var buffer_size: int = multimesh.instance_count * 8
	var buffer: PackedFloat32Array = ProjectileSystem.get_buffer(buffer_size)
	
	multimesh.buffer = buffer
	multimesh.visible_instance_count = ProjectileSystem.get_projectile_count()
