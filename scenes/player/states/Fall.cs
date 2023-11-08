namespace SharpGame;

[GlobalClass]
public partial class Fall : State
{
	[Export]
	public float Speed = 150.0f;

	[Export]
	public float Acceleration = 4.0f;

	[Export]
	State _idle;

	[Export]
	State _walk;

	[Export]
	State _wallSlide;

	float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override State PhysicsProcess(double delta)
	{
		float input = Godot.Input.GetAxis("move_left", "move_right");

		Vector2 velocity = CharacterBody2D.Velocity;
		velocity.Y += Gravity * (float)delta;
		velocity.X = Mathf.Lerp(velocity.X, Speed * input, Acceleration * (float)delta);

		CharacterBody2D.Velocity = velocity;
		CharacterBody2D.MoveAndSlide();

		if (CharacterBody2D.IsOnFloor())
		{
			if (!Mathf.IsZeroApprox(input))
				return _walk;

			return _idle;
		}

		if (CharacterBody2D.IsOnWall())
			return _wallSlide;

		return null;
	}
}
