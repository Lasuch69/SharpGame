using Godot;
using System;

public sealed partial class Player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float Acceleration = 16.0f;
	public const float JumpVelocity = -150.0f;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

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

		if (IsOnFloor())
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, Acceleration * (float)delta);
		else
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, (Acceleration / 5.0f) * (float)delta);

		Velocity = velocity;
		MoveAndSlide();
	}
}
