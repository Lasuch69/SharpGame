using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float Acceleration = 16.0f;
	public const float JumpVelocity = -150.0f;
	private Sprite2D _sprite;
	
	private float _physicsDelta = 1.0f / ProjectSettings.GetSetting("physics/common/physics_ticks_per_second").AsSingle(); 

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
	}

	public override void _Process(double delta)
	{
		// Sprite movement interpolation.
		_sprite.Position = Vector2.Zero.Lerp(Velocity * _physicsDelta, (float)Engine.GetPhysicsInterpolationFraction());
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, Acceleration * (float)delta);

		Velocity = velocity;
		MoveAndSlide();
	}
}
