using Godot;
using System;

public partial class PhysicsInterpolation : Node2D
{
	private CharacterBody2D _parent;
	private float _physicsDelta;

	public override void _Ready()
	{
		_parent = GetParent<CharacterBody2D>();
	}

	public override void _PhysicsProcess(double delta)
	{
		_physicsDelta = (float)delta;
	}

	public override void _Process(double delta)
	{
		Vector2 newPosition = _parent.Velocity * _physicsDelta;
		float physicsInterpolationFraction = (float)Engine.GetPhysicsInterpolationFraction();

		this.Position = Vector2.Zero.Lerp(newPosition, physicsInterpolationFraction);
	}
}
