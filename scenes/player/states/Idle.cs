namespace SharpGame;

[GlobalClass]
public partial class Idle : State
{
	[Export]
	public float Acceleration = 12.0f;

	[Export]
	State _fall;

	[Export]
	State _jump;

	[Export]
	State _walk;

	public override State PhysicsProcess(double delta)
	{
		CharacterBody2D.Velocity = CharacterBody2D.Velocity.Lerp(Vector2.Zero, Acceleration * (float)delta);
		CharacterBody2D.MoveAndSlide();

		float input = Godot.Input.GetAxis("move_left", "move_right");

		if (!Mathf.IsZeroApprox(input))
			return _walk;

		if (!CharacterBody2D.IsOnFloor())
			return _fall;

		return null;
	}

	public override State Input(InputEvent @event)
	{
		if (@event.IsAction("jump") && @event.IsPressed())
			return _jump;

		return null;
	}
}
