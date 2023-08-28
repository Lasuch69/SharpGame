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
		Vector2 velocity = Entity.Velocity;
		velocity.Y = -JumpForce;
		Entity.Velocity = velocity;
	}

	public override State PhysicsProcess(double delta)
	{
		Vector2 velocity = Entity.Velocity;
		velocity.Y += Gravity * (float)delta;
		Entity.Velocity = velocity;
		Entity.MoveAndSlide();

		if (!Godot.Input.IsActionPressed("jump") || Entity.Velocity.Y > 0.0f)
			return _fall;

		return null;
	}
}
