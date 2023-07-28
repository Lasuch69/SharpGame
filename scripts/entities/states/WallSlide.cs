namespace SharpGame;

[GlobalClass]
public partial class WallSlide : State
{
    [Export]
    public float Acceleration = 6.0f;

    [Export]
    public float SlideSpeed = 75.0f;

    [Export]
    public float SnapLength = 8.0f;

    [Export]
    private State _fall;

    [Export]
    private State _idle;

    [Export]
    private State _walk;

    private PhysicsDirectSpaceState2D _spaceState = null;
    private Vector2 _wallNormal = Vector2.Zero;
    private bool _jumpOff = false;

    public override void Enter()
    {
        _spaceState ??= PhysicsServer2D.SpaceGetDirectState(Entity.GetWorld2D().Space);
        _wallNormal = Entity.GetWallNormal();

        _jumpOff = false;
    }

    public override State PhysicsProcess(double delta)
    {
        Vector2 velocity = Entity.Velocity;

        velocity.X = 0.0f;
        velocity.Y = Mathf.Lerp(velocity.Y, SlideSpeed, Acceleration * (float)delta);

        Entity.Velocity = velocity;

        if (_jumpOff)
            Entity.Velocity = new Vector2(125.0f * _wallNormal.X, -125.0f);

        Entity.MoveAndSlide();

        if (Entity.IsOnFloor())
        {
            float input = Godot.Input.GetAxis("move_left", "move_right");

            if (!Mathf.IsZeroApprox(input))
                return _walk;

            return _idle;
        }

        bool isColliding = IsColliding(Entity.Position, Entity.Position + -_wallNormal * SnapLength);

        if (!isColliding)
            return _fall;

        return null;
    }

    public override State Input(InputEvent @event)
    {
        if (@event.IsAction("jump") && @event.IsPressed())
            _jumpOff = true;

        return null;
    }

    private bool IsColliding(Vector2 from, Vector2 to)
    {
        PhysicsRayQueryParameters2D parameters = new()
        {
            From = from,
            To = to
        };
        parameters.Exclude.Add(Entity.GetRid());

        return _spaceState.IntersectRay(parameters).Count != 0;
    }
}
