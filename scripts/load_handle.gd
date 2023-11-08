class_name LoadHandle extends RefCounted

signal resource_loaded(path: String, resource: Resource)
signal resource_updated(path: String, progress: float)
