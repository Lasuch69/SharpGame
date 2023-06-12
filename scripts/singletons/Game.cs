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
			_player = value;
			
			EmitSignal(SignalName.PlayerChanged, _player);
		}
	}

	public int Score
	{
		get => _score;
		set {
			_score = value;
			
			EmitSignal(SignalName.ScoreChanged, _score);
		}
	}

	private Player _player;
	private int _score;

	private Geometry _geometry = new ();
	private Timer _spawnTimer = new ();

	private Level _level;
	private Vector2[] _verticies;
	private int[] _indices;

	private PackedScene _enemyScene = (PackedScene)GD.Load("res://scenes/flyer.tscn");

	private int _currentWave = 0;
	private List<PackedScene> _spawnQueue = new ();
	private List<CharacterBody2D> _waveEnemies = new ();

	public override void _Ready()
	{
		_spawnTimer.WaitTime = 1.0f;
		_spawnTimer.ProcessCallback = Timer.TimerProcessCallback.Physics;

		_spawnTimer.Timeout += OnSpawnTimerTimeout;

		AddChild(_spawnTimer);
	}

	public void SetLevel(Level level)
	{
		_level = level;
		_verticies = _level.NavigationMap.NavigationPolygon.GetOutline(0);
		_indices = Geometry2D.TriangulatePolygon(_verticies);
	}

	private void StartWave()
	{
		_currentWave++;

		for (int i = 0; i < _currentWave * 2; i++)
		{
			_spawnQueue.Add(_enemyScene);
		}

		_spawnTimer.Start();

		EmitSignal(SignalName.WaveStarted, _currentWave);
	}

	private void FinishWave()
	{
		EmitSignal(SignalName.WaveFinished, _currentWave);
	}

	private void OnSpawnTimerTimeout()
	{
		if (_spawnQueue.Count == 0)
		{
			_spawnTimer.Stop();
			
			return;
		}

		int index = _spawnQueue.Count - 1;
		PackedScene enemyScene = _spawnQueue[index];
		_spawnQueue.RemoveAt(index);

		CharacterBody2D enemy = (CharacterBody2D)enemyScene.Instantiate();
		
		Vector2 spawnPoint = GetSpawnPoint(_verticies, _indices, 256.0f);
		enemy.Position = spawnPoint;
		
		HealthComponent enemyHealth = enemy.GetNode<HealthComponent>("HealthComponent");

		_waveEnemies.Add(enemy);
		
		enemyHealth.HealthChanged += (newHealth, oldHealth, instigator) =>
		{
			if (newHealth > 0)
				return;

			_waveEnemies.Remove(enemy);

			if (_waveEnemies.Count == 0 && _spawnQueue.Count == 0)
				FinishWave();
		};

		GetTree().CurrentScene.AddChild(enemy);
	}

	private float CalculateCost(Vector2 point, Vector2 target, float desiredDistance)
	{
		float distance = point.DistanceTo(target);
		return Mathf.Abs(distance - desiredDistance);
	}

	private Vector2 GetSpawnPoint(Vector2[] verticies, int[] indices, float desiredDistance = 128.0f, int iterations = 16)
	{
		Vector2[] points = _geometry.GetRandomPointsInPolygon(verticies, indices, iterations);

		float[] costs = new float[iterations];

		for (int i = 0; i < iterations; i++)
		{
			costs[i] = CalculateCost(points[i], _player.Position, desiredDistance);
		}

		float lowestCost = float.MaxValue;
		int lowestCostIndex = 0;

		for (int i = 0; i < iterations; i++)
		{
			if (costs[i] < lowestCost)
			{
				lowestCost = costs[i];
				lowestCostIndex = i;
			}
		}

		return points[lowestCostIndex];
	}	
}
