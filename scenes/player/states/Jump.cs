namespace SharpGame;

[GlobalClass]
public partial class Jump : State
{
	[Export]
	public float JumpForce = 127.5f;

	[Export]
	State _fall;

	float Gravity = 245.0f;

	public override void Enter()
	{
		Vector2 velocity = CharacterBody2D.Velocity;
		velocity.Y = -JumpForce;
		CharacterBody2D.Velocity = velocity;
	}

	public override State PhysicsProcess(double delta)
	{
		Vector2 velocity = CharacterBody2D.Velocity;
		velocity.Y += Gravity * (float)delta;
		CharacterBody2D.Velocity = velocity;
		CharacterBody2D.MoveAndSlide();

		if (!Godot.Input.IsActionPressed("jump") || CharacterBody2D.Velocity.Y > 0.0f)
			return _fall;

		return null;
	}
}
