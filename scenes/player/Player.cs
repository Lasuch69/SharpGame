namespace SharpGame;

[GlobalClass]
public sealed partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void ScoreChangedEventHandler(int score);

	[Export]
	public HealthComponent HealthComponent;

	[Export]
	public ProjectileComponent ProjectileComponent;

	public override void _Input(InputEvent @event)
	{
		if (!@event.IsActionPressed("shoot"))
			return;

		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 direction = Position.DirectionTo(mousePosition);

		Vector2 position = direction * 12.0f + Position;
		Vector2 velocity = direction * 256.0f;

		ProjectileComponent.SpawnProjectile(position, velocity, GetParent());
	}

	void OnCollision(Vector2 position, Godot.Collections.Dictionary result)
	{
		Node collider = (Node)InstanceFromId((ulong)result["collider_id"]);

		if (collider is not Flyer)
			return;

		Flyer flyer = (Flyer)collider;
		flyer.HealthComponent.Damage(1);

		EmitSignal(SignalName.ScoreChanged, flyer.ScoreOnKill);
	}
}
