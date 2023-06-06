using Godot;
using System;

public sealed partial class Player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float Acceleration = 16.0f;
	public const float JumpVelocity = -150.0f;

	[Export]
	public HealthComponent HealthComponent;

	[Export]
	public PackedScene Projectile;

	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		Game game = GetNode<Game>("/root/Game");
		game.Player = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (IsOnFloor())
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, Acceleration * (float)delta);
		else
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, (Acceleration / 5.0f) * (float)delta);

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton && @event.IsPressed())
		{
			Projectile projectile = (Projectile)Projectile.Instantiate();
			
			Vector2 mousePosition = GetGlobalMousePosition();
			
			projectile.Position = (this.Position.DirectionTo(mousePosition) * 12.0f) + this.Position;
			projectile.Velocity = this.Position.DirectionTo(mousePosition) * 256.0f;
			
			projectile.HitboxComponent.TargetGroup = "Enemy";
			projectile.InstancedBy = this;
			
			GetParent().AddChild(projectile);
		}
	}
}
