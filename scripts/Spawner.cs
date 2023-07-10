using Godot;
using System;

public partial class Spawner : Node2D
{
	[Export]
	private Player _player = null;

	[Export]
	private Timer _timer = null;

	private PhysicsDirectSpaceState2D _spaceState = null;
	private PackedScene _enemyScene = (PackedScene)GD.Load("res://scenes/flyer.tscn");
	
	private Random _random = new ();
	
	public override void _Ready()
	{
		_timer.Timeout += () => Spawn(_enemyScene);
		_timer.Start();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_spaceState == null)
			_spaceState = PhysicsServer2D.SpaceGetDirectState(GetWorld2D().Space);
	}

	private Node2D Spawn(PackedScene scene)
	{
		Node instance = scene.Instantiate();

		if (!instance.IsClass("Node2D"))
			return null;

		if (_player == null)
			return null;

		Vector2 position = Vector2.Zero;

		int i = 0;

		while (i < 32)
		{
			Vector2 point = GetRandomPoint(128.0f) + _player.Position;

			PhysicsRayQueryParameters2D parameters = new ();
			
			parameters.From = _player.Position;
			parameters.To = point;
			parameters.Exclude.Add(_player.GetRid());

			if (_spaceState.IntersectRay(parameters).Count == 0)
			{
				position = point;
				break;
			}

			i++;
		}
		
		Node2D entity = (Node2D)instance;

		entity.Position = position;

		GetParent().AddChild(entity);

		return entity;
	}

	private Vector2 GetRandomPoint(float desiredDistance)
	{
		float a = _random.NextSingle() * 2 * MathF.PI;

		float i = _random.NextSingle() * 0.25f;
		i -= i / 2.0f;

		float r = desiredDistance * (1.0f + i);

		return new Vector2(r * MathF.Cos(a), r * MathF.Sin(a));
	}
}
