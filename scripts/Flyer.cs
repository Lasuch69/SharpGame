using Godot;
using System;

public partial class Flyer : CharacterBody2D
{
	public const float Speed = 70.0f;
	
	public Node2D Target;
	
	[Export]
	public DamageComponent DamageComponent;
	
	[Export]
	public HealthComponent HealthComponent;

	[Export]
	public HitboxComponent HitboxComponent;
	
	[Export]
	public NavigationAgent2D NavigationAgent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NavigationAgent.VelocityComputed += _OnVelocityComputed;
		NavigationAgent.TargetPosition = Position;

		Game game = GetNode<Game>("/root/Game");
		Target = game.Player;
		game.PlayerChanged += (player) => Target = player;

		HitboxComponent.TargetEntered += (target) =>
		{
			HealthComponent targetHealthComponent = target.GetNode<HealthComponent>("HealthComponent");
			DamageComponent.ApplyDamage(targetHealthComponent);
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
