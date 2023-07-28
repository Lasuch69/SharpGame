namespace SharpGame;

[GlobalClass]
public partial class Fall : State
{
    [Export]
    public float Speed = 150.0f;

    [Export]
    public float Acceleration = 4.0f;

    [Export]
    private State _idle;

    [Export]
    private State _walk;

    [Export]
    private State _wallSlide;

    private float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override State PhysicsProcess(double delta)
    {
        float input = Godot.Input.GetAxis("move_left", "move_right");

        Vector2 velocity = Entity.Velocity;
        velocity.Y += Gravity * (float)delta;
        velocity.X = Mathf.Lerp(velocity.X, Speed * input, Acceleration * (float)delta);

        Entity.Velocity = velocity;
        Entity.MoveAndSlide();

        if (Entity.IsOnFloor())
        {
            if (!Mathf.IsZeroApprox(input))
                return _walk;

            return _idle;
        }

        if (Entity.IsOnWall())
            return _wallSlide;

        return null;
    }
}
