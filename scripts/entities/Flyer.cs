using Godot;
using System;

public partial class Flyer : CharacterBody2D
{
	public const float Speed = 70.0f;
	
	public Node2D Target;
	
	[Export]
	public int ScoreOnKill = 100;

	[Export]
	public DamageComponent DamageComponent;
	
	[Export]
	public HealthComponent HealthComponent;

	[Export]
	public HitboxComponent HitboxComponent;
	
	[Export]
	public NavigationAgent2D NavigationAgent;

	private Game _game;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NavigationAgent.VelocityComputed += _OnVelocityComputed;
		NavigationAgent.TargetPosition = Position;

		_game = GetNode<Game>("/root/Game");
		Target = _game.Player;
		_game.PlayerChanged += (player) => Target = player;

		HitboxComponent.TargetEntered += (target) =>
		{
			HealthComponent targetHealthComponent = target.GetNode<HealthComponent>("HealthComponent");
			DamageComponent.ApplyDamage(targetHealthComponent, this);
		};

		HealthComponent.HealthChanged += (newHealth, oldHealth, instigator) => 
		{
			if (newHealth == 0)
			{
				if (instigator == _game.Player)
					_game.Score += ScoreOnKill;
				
				QueueFree();
			}
		};
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Target != null)
			NavigationAgent.TargetPosition = Target.Position;

		Vector2 pathPosition = NavigationAgent.GetNextPathPosition();

		Vector2 direction = Position.DirectionTo(pathPosition);
		Vector2 velocity = direction * Speed;
		
		if (NavigationAgent.AvoidanceEnabled)
			NavigationAgent.SetVelocity(velocity);
		else
			Velocity = velocity;

		MoveAndSlide();
	}

	private void _OnVelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
	}
}
