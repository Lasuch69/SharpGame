using Godot;
using System;
using System.Collections.Generic;

public partial class Spawner : Node
{
	[Export]
	public Node SpawnRoot;

	[Export]
	public NavigationRegion2D NavigationMap
	{
		get => _navigationMap;
		set
		{
			Vector2[] polygon = value.NavigationPolygon.GetOutline(0);
			int[] indices = Geometry2D.TriangulatePolygon(polygon);

			_navigationMap = value;
			_polygon = polygon;
			_indices = indices;
		}
	}

	private NavigationRegion2D _navigationMap;
	private Vector2[] _polygon = new Vector2[0];
	private int[] _indices = new int[0];

	private TriangleMath _triangleMath = new ();
	private Timer _timer = new ();

	private List<Node2D> _spawnedEntities = new ();

	private Game _game;

	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Game");

		_timer.WaitTime = 1.5f;
		_timer.ProcessCallback = Timer.TimerProcessCallback.Physics;

		_timer.Timeout += OnTimerTimeout;

		AddChild(_timer);

		_game.StartWave();
		_timer.Start();
	}

	private void OnTimerTimeout()
	{
		int queueSize = _game.SpawnQueue.Count;

		if (queueSize == 0)
		{
			_timer.Stop();
			return;
		}

		Spawn(_game.SpawnQueue[queueSize - 1]);
		_game.SpawnQueue.RemoveAt(queueSize - 1);
	}

	private void Spawn(PackedScene scene)
	{
		Node2D entity = (Node2D)scene.Instantiate();
		entity.Position = GetSpawnPoint();

		HealthComponent entityHealth = entity.GetNodeOrNull<HealthComponent>("HealthComponent");

		SpawnRoot.AddChild(entity);

		if (entityHealth == null)
			return;

		entityHealth.HealthChanged += (newHealth, oldHealth, instigator) =>
		{
			if (newHealth > 0)
				return;

			EntityKilled(entity);
		};

		_spawnedEntities.Add(entity);
	}

	private void EntityKilled(Node2D entity)
	{
		_spawnedEntities.Remove(entity);

		if (_spawnedEntities.Count == 0 && _game.SpawnQueue.Count == 0)
		{
			_game.FinishWave();
			_game.StartWave();
			_timer.Start();
		}
	}

	private float CalculateCost(Vector2 point, Vector2 target, float desiredDistance)
	{
		return Mathf.Abs(point.DistanceTo(target) - desiredDistance);
	}

	private Vector2 GetSpawnPoint(float desiredDistance = 128.0f, int iterations = 16)
	{
		TriangleMath.Triangle[] triangles = _triangleMath.GetTriangles(_polygon, _indices);
		float area = _triangleMath.GetTrianglesArea(triangles);

		Vector2[] points = new Vector2[iterations];
		float[] costs = new float[iterations];

		for (int i = 0; i < iterations; i++)
		{
			Vector2 point = _triangleMath.PickRandomTriangle(triangles, area).GetRandomPoint();
			float cost = CalculateCost(points[i], _game.Player.Position, desiredDistance);

			points[i] = point;
			costs[i] = cost;
		}

		float lowestCost = float.MaxValue;
		int lowestCostIndex = 0;

		for (int i = 0; i < iterations; i++)
		{
			float cost = costs[i];

			if (cost > lowestCost)
				continue;

			lowestCost = cost;
			lowestCostIndex = i;
		}

		return points[lowestCostIndex];
	}	
}
