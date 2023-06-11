using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void PlayerChangedEventHandler(Player player);

	[Signal]
	public delegate void ScoreChangedEventHandler(int score);

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

	private Geometry _geometry;

	public override void _Ready()
	{
		_geometry = new Geometry();
	}

	public Vector2 GetSpawnPoint(Vector2[] verticies, int[] indices, float desiredDistance = 128.0f, int iterations = 16)
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

	private float CalculateCost(Vector2 point, Vector2 target, float desiredDistance)
	{
		float distance = point.DistanceTo(target);
		return Mathf.Abs(distance - desiredDistance);
	}
}
