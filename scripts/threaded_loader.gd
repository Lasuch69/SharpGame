class_name ThreadedLoader extends Node

@onready
var _timer := Timer.new()
var _queue: Dictionary = {}

func _ready() -> void:
	_timer.timeout.connect(_on_timeout)
	
	_timer.autostart = false
	_timer.wait_time = 0.25
	_timer.one_shot = false
	
	add_child(_timer)

func request_resource(path: String, type_hint: String = "", use_sub_threads: bool = true) -> LoadHandle:
	var handle := LoadHandle.new()
	_queue[path] = handle
	
	var err = ResourceLoader.load_threaded_request(path, type_hint, use_sub_threads)
	print_verbose("ThreadedLoader: %s requested... %s" % [path, error_string(err)])
	
	if _timer.is_stopped():
		_timer.start()
	
	return handle

func _on_timeout() -> void:
	for path in _queue.keys():
		var progress: Array = []
		var status := ResourceLoader.load_threaded_get_status(path, progress)
		
		match status:
			ResourceLoader.THREAD_LOAD_INVALID_RESOURCE:
				push_error("path \"%s\" is invalid!" % path)
				_queue[path].resource_loaded.emit(path, null)
				_queue.erase(path)
			ResourceLoader.THREAD_LOAD_IN_PROGRESS:
				_queue[path].resource_updated.emit(path, progress[0])
			ResourceLoader.THREAD_LOAD_FAILED:
				push_error("resource at: \"%s\" failed to load!" % path)
				_queue[path].resource_loaded.emit(path, null)
				_queue.erase(path)
			ResourceLoader.THREAD_LOAD_LOADED:
				var resource: Resource = ResourceLoader.load_threaded_get(path)
				_queue[path].resource_loaded.emit(path, resource)
				_queue.erase(path)
	
	if _queue.is_empty():
		_timer.stop()
