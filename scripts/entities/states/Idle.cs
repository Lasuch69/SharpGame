using Godot;
using System;

[GlobalClass]
public partial class Idle : State
{
    [Export]
    public float Acceleration = 12.0f;

    [Export]
    private State _fall;

    [Export]
    private State _jump;

    [Export]
    private State _walk;

    public override State PhysicsProcess(double delta)
    {
        Entity.Velocity = Entity.Velocity.Lerp(Vector2.Zero, Acceleration * (float)delta);
        Entity.MoveAndSlide();

        float input = Godot.Input.GetAxis("move_left", "move_right");

        if (!Mathf.IsZeroApprox(input))
            return _walk;

        if (!Entity.IsOnFloor())
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
