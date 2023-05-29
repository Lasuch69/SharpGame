using Godot;
using System;

public partial class Flyer : CharacterBody2D
{
	public const float Speed = 70.0f;
	
	private Node2D _target;
	
	private DamageComponent _damage;
	private HitboxComponent _hitbox;
	
	private NavigationAgent2D _navigationAgent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_navigationAgent.VelocityComputed += _OnVelocityComputed;
		_navigationAgent.TargetPosition = Position;

		Game game = GetNode<Game>("/root/Game");
		_target = game.Player;
		game.PlayerChanged += (player) => _target = player;

		_damage = GetNode<DamageComponent>("DamageComponent");
		_hitbox = GetNode<HitboxComponent>("HitboxComponent");

		_hitbox.TargetEntered += (target) =>
		{
			HealthComponent targetHealthComponent = target.GetNode<HealthComponent>("HealthComponent");
			_damage.ApplyDamage(targetHealthComponent);
		};
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_target != null)
			_navigationAgent.TargetPosition = _target.Position;

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
