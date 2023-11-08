namespace SharpGame;

[GlobalClass]
public partial class Projectile : Area2D
{
	[Signal]
	public delegate void OnCollisionEventHandler(Node body, Vector2 position, Vector2 velocity);

	public Vector2 Velocity;

	[Export]
	Sprite2D _projectileSprite;

	public override void _Ready() => BodyEntered += (body) => Collided(body);

	public override void _PhysicsProcess(double delta) => Position += Velocity * (float)delta;

	public override void _Process(double delta)
	{
		float interpolationFraction = (float)Engine.GetPhysicsInterpolationFraction();
		float physicsDelta = (float)GetPhysicsProcessDeltaTime();

		_projectileSprite.Position = Vector2.Zero.Lerp(Velocity * physicsDelta, interpolationFraction);
	}

	void Collided(Node body)
	{
		EmitSignal(SignalName.OnCollision, body, Position, Velocity);
		OnCollisionCallback(body);
	}

	protected virtual void OnCollisionCallback(Node body)
	{
		QueueFree();
	}
}
