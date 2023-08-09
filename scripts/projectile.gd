class_name Projectile
extends RefCounted

var position := Vector2.ZERO
var velocity := Vector2.ZERO

var collision_mask: int = 1

var callback: Callable
