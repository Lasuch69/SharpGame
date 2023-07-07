using Godot;
using System;

public partial class Spawner : Node
{
	private Game _game;
	private Godot.Collections.Array<Marker2D> _markers = new ();
	
	public int Wave { get => _wave; }

	private int _score = 0;
	private int _wave = 0;
	
	private Player _player = null;

	private Godot.Collections.Array<PackedScene> _spawnQueue = new ();
	private Godot.Collections.Array<Node> _entities = new ();

	private Timer _timer = new ();

	private PackedScene _enemyScene = (PackedScene)GD.Load("res://scenes/flyer.tscn");

	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Game");
		
		foreach (Node node in GetChildren())
		{
			if (!node.IsClass("Marker2D"))
				continue;

			_markers.Add((Marker2D)node);
		}
		
		_timer.WaitTime = 2.0f;
		_timer.ProcessCallback = Timer.TimerProcessCallback.Physics;
		_timer.Timeout += OnTimerTimeout;

		AddChild(_timer);

		StartWave();
	}

	private void OnTimerTimeout()
	{
		if (_spawnQueue.Count == 0)
		{
			_timer.Stop();
			return;
		}

		int idx = _spawnQueue.Count - 1;
		PackedScene scene = _spawnQueue[idx];
		_spawnQueue.RemoveAt(idx);
		
		Node2D entity = Spawn(scene);

		if (entity == null)
			return;
		
		entity.GetNode<HealthComponent>("HealthComponent").HealthEmpty += (instigator) => 
			OnEntityKilled(entity);
		
		_entities.Add(entity);
	}

	public Node2D Spawn(PackedScene scene)
	{
		if (_game.Player == null)
			return null;

		Vector2 targetPosition = _game.Player.Position;
		
		float[] costs = new float[_markers.Count];

		for (int i = 0; i < _markers.Count; i++)
		{
			costs[i] = CalculateCost(_markers[i].Position, targetPosition);
		}

		int idx = 0;

		for (int i = 0; i < costs.Length; i++)
		{
			if (costs[i] > costs[idx])
				continue;

			idx = i;
		}
		
		Node2D entity = (Node2D)scene.Instantiate();
		entity.Position = _markers[idx].Position;
		GetTree().CurrentScene.AddChild(entity);

		return entity;
	}

	private void OnEntityKilled(Node2D entity)
	{
		_entities.Remove(entity);

		if (_spawnQueue.Count != 0 || _entities.Count != 0)
			return;

		FinishWave();
		StartWave();
	}

	private void StartWave()
	{
		_wave++;
		GD.Print("Wave: ", _wave);

		_spawnQueue = GenerateSpawnQueue(_wave);

		_timer.Start();
	}

	private void FinishWave()
	{
		GD.Print("Wave finished!");
	}

	private int GetWaveThreatPoints(int wave)
	{
		double value = ((wave - 1) % 5) + (wave / 5) + 1;
		value *= value;

		return (int)value;
	}
	
	private Godot.Collections.Array<PackedScene> GenerateSpawnQueue(int wave)
	{
		int threatPoints = GetWaveThreatPoints(wave);
		Godot.Collections.Array<PackedScene> queue = new ();

		for (int i = 0; i < threatPoints; i++)
		{
			queue.Add(_enemyScene);
		}

		return queue;
	}

	private float CalculateCost(Vector2 from, Vector2 to, float desiredDistance = 128.0f)
	{
		return Mathf.Abs(from.DistanceTo(to) - desiredDistance);
	}
}
