namespace SharpGame;

public partial class Dead : State
{
    float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override State PhysicsProcess(double delta)
    {
        Vector2 velocity = Entity.Velocity;

        velocity.X = Mathf.Lerp(velocity.X, 0.0f, 6.0f * (float)delta);
        velocity.Y += Gravity * (float)delta;

        Entity.Velocity = velocity;
        Entity.MoveAndSlide();

        return null;
    }
}
