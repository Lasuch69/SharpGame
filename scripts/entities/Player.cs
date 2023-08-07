namespace SharpGame;

[GlobalClass]
public sealed partial class Player : CharacterBody2D
{
    [Export]
    public HealthComponent HealthComponent;

    [Export]
    public PackedScene Projectile;

    public override void _Ready()
    {
        Game game = GetNode<Game>("/root/Game");
        game.Player = this;
    }

    public override void _Input(InputEvent @event)
    {
        if (!@event.IsActionPressed("shoot"))
            return;

        var projectile = (Projectile)Projectile.Instantiate();

        Vector2 mousePosition = GetGlobalMousePosition();
        Vector2 direction = Position.DirectionTo(mousePosition);

        projectile.Position = direction * 12.0f + Position;
        projectile.Velocity = direction * 256.0f;

        projectile.HitboxComponent.TargetGroups.Add("Enemy");

        GetParent().AddChild(projectile);
    }
}
