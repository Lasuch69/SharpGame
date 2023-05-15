using Godot;
using System;

public partial class flyer : CharacterBody2D
{
	public const float Speed = 70.0f;
	
	[Export]
	private CharacterBody2D _player;
	
	private AnimatedSprite2D _sprite;
	private NavigationAgent2D _navigationAgent;
	
	private float _physicsDelta = 1.0f / ProjectSettings.GetSetting("physics/common/physics_ticks_per_second").AsSingle(); 


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		
		_navigationAgent.VelocityComputed += _OnVelocityComputed;

		_navigationAgent.TargetPosition = Position;
	}

	public override void _Process(double delta)
	{
		// Sprite movement interpolation.
		_sprite.Position = Vector2.Zero.Lerp(Velocity * _physicsDelta, (float)Engine.GetPhysicsInterpolationFraction());
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
