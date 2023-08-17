extends VisualEffect

var destroy_on_finish: bool = true

func start() -> void:
	$GPUParticles2D.restart()
	$Timer.start()

func _on_timer_timeout() -> void:
	effect_finished.emit()
	
	if destroy_on_finish:
		queue_free()
