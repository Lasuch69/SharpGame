[GlobalClass]
public partial class Jump : State
{
    [Export]
    public float JumpForce = 150.0f;

    [Export]
    private State _fall;

    private float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

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

        if (Entity.Velocity.Y > 0.0f)
            return _fall;

        return null;
    }
}
