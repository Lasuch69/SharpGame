namespace SharpGame;

[GlobalClass]
public partial class Walk : State
{
    [Export]
    public float Speed = 150.0f;

    [Export]
    public float Acceleration = 12.0f;

    [Export]
    State _fall;

    [Export]
    State _idle;

    [Export]
    State _jump;

    double _fallTime = 0.0;

    float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void Enter()
    {
        _fallTime = 0.0;
    }

    public override State PhysicsProcess(double delta)
    {
        float input = Godot.Input.GetAxis("move_left", "move_right");

        Vector2 velocity = Entity.Velocity;

        if (!Entity.IsOnFloor())
            velocity.Y += Gravity * (float)delta;

        velocity.X = Mathf.Lerp(velocity.X, Speed * input, Acceleration * (float)delta);

        Entity.Velocity = velocity;
        Entity.MoveAndSlide();

        if (!Entity.IsOnFloor())
        {
            _fallTime += delta;

            if (_fallTime > 0.1)
                return _fall;
        }
        else
            _fallTime = 0.0;

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
