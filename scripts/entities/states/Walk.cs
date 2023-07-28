namespace SharpGame;

[GlobalClass]
public partial class Walk : State
{
    [Export]
    public float Speed = 150.0f;

    [Export]
    public float Acceleration = 12.0f;

    [Export]
    private State _fall;

    [Export]
    private State _idle;

    [Export]
    private State _jump;

    public override State PhysicsProcess(double delta)
    {
        float input = Godot.Input.GetAxis("move_left", "move_right");

        var velocity = new Vector2(Speed * input, 0.0f);
        Entity.Velocity = Entity.Velocity.Lerp(velocity, Acceleration * (float)delta);
        Entity.MoveAndSlide();

        if (!Entity.IsOnFloor())
            return _fall;

        if (Mathf.IsZeroApprox(input))
            return _idle;

        return null;
    }

    public override State Input(InputEvent @event)
    {
        if (@event.IsAction("jump") && @event.IsPressed())
            return _jump;

        return null;
    }
}
