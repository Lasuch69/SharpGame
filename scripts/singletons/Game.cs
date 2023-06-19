using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	[Signal]
	public delegate void PlayerChangedEventHandler(Player player);

	[Signal]
	public delegate void ScoreChangedEventHandler(int score);

	[Signal]
	public delegate void WaveStartedEventHandler(int wave);

	[Signal]
	public delegate void WaveFinishedEventHandler(int wave);

	public Player Player
	{
		get => _player;
		set
		{
			if (_player == value)
				return;
			
			_player = value;
			EmitSignal(SignalName.PlayerChanged, _player);
		}
	}

	public int Score
	{
		get => _score;
		set
		{
			if (_score == value)
				return;
			
			_score = value;
		   	EmitSignal(SignalName.ScoreChanged, _score);
		}
	}

	public Spawner Spawner { 
		get => _spawner; 
		set
		{
			_spawner = value;
			
			if (_spawner != null)
				StartWave();
		}
	}

	public int Wave { get => _wave; }

	private int _score = 0;
	private int _wave = 0;
	
	private Player _player = null;
	private Spawner _spawner = null;

	private List<PackedScene> _spawnQueue = new ();
	private List<Node> _entities = new ();

	private Timer _timer = new ();

	private PackedScene _enemyScene = (PackedScene)GD.Load("res://scenes/flyer.tscn");

	public override void _Ready()
	{
		_timer.WaitTime = 2.0f;
		_timer.ProcessCallback = Timer.TimerProcessCallback.Physics;
		_timer.Timeout += OnTimerTimeout;

		AddChild(_timer);
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
		
		Node2D entity = Spawner.Spawn(scene);

		if (entity == null)
			return;
		
		entity.GetNode<HealthComponent>("HealthComponent").HealthEmpty += (instigator) => 
			OnEntityKilled(entity);
		
		_entities.Add(entity);
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

		EmitSignal(SignalName.WaveStarted, _wave);
	}

	private void FinishWave()
	{
		GD.Print("Wave finished!");
		EmitSignal(SignalName.WaveFinished, _wave);
	}

	private int GetWaveThreatPoints(int wave)
	{
		double value = ((wave - 1) % 5) + (wave / 5) + 1;
		value *= value;

		return (int)value;
	}
	
	private List<PackedScene> GenerateSpawnQueue(int wave)
	{
		int threatPoints = GetWaveThreatPoints(wave);
		List<PackedScene> queue = new ();

		for (int i = 0; i < threatPoints; i++)
		{
			queue.Add(_enemyScene);
		}

		return queue;
	}
}
