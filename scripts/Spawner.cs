using Godot;
using System;

public partial class Spawner : Node
{
	private Game _game;
	private Godot.Collections.Array<Marker2D> _markers = new ();

	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Game");
		_game.Spawner = this;
		
		foreach (Node node in GetChildren())
		{
			if (!node.IsClass("Marker2D"))
				continue;

			_markers.Add((Marker2D)node);
		}
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

	private float CalculateCost(Vector2 from, Vector2 to, float desiredDistance = 128.0f)
	{
		return Mathf.Abs(from.DistanceTo(to) - desiredDistance);
	}
}
