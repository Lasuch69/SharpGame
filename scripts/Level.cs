using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D
{
	[Export]
	private NavigationRegion2D _navigationRegion;

	[Export]
	private Timer _spawnTimer;

	[Export]
	private PackedScene _enemy;

	private Game _game;

	public override void _Ready()
	{
		_spawnTimer.Timeout += OnSpawnTimerTimeout;
		_game = GetNode<Game>("/root/Game");
	}

	private void OnSpawnTimerTimeout()
	{
		Vector2[] verticies = _navigationRegion.NavigationPolygon.GetOutline(0);
		int[] indices = Geometry2D.TriangulatePolygon(verticies);

		Vector2 spawnPoint = _game.GetSpawnPoint(verticies, indices);

		Node2D enemy = (Node2D)_enemy.Instantiate();
		enemy.Position = spawnPoint;
		GetTree().CurrentScene.AddChild(enemy);
	}
}
