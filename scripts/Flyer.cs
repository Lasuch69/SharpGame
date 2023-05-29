using Godot;
using System;

public partial class Flyer : CharacterBody2D
{
	public const float Speed = 70.0f;
	
	[Export]
	private CharacterBody2D _player;
	
	private NavigationAgent2D _navigationAgent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		
		_navigationAgent.VelocityComputed += _OnVelocityComputed;

		_navigationAgent.TargetPosition = Position;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_navigationAgent.TargetPosition = _player.Position;

		Vector2 pathPosition = _navigationAgent.GetNextPathPosition();

		Vector2 direction = Position.DirectionTo(pathPosition);
		Vector2 velocity = direction * Speed;
		
		if (_navigationAgent.AvoidanceEnabled)
			_navigationAgent.SetVelocity(velocity);
		else
			Velocity = velocity;

		MoveAndSlide();
	}

	private void _OnVelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
	}
}
