namespace SharpGame;

[GlobalClass]
public sealed partial class Player : CharacterBody2D
{
    [Export]
    public HealthComponent HealthComponent;

    [Export]
    public PackedScene Projectile;

    [Export]
    AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        Game game = GetNode<Game>("/root/Game");
        game.Player = this;

        HealthComponent.HealthChanged += (newHealth, oldHealth) =>
        {
            if (newHealth < oldHealth)
                AnimationPlayer.Play("damaged");
        };
    }

    public override void _Input(InputEvent @event)
    {
        Shoot(@event);
    }

    void Shoot(InputEvent @event)
    {
        if (!@event.IsActionPressed("shoot"))
            return;

        var projectile = (Projectile)Projectile.Instantiate();

        Vector2 mousePosition = GetGlobalMousePosition();

        projectile.Position = Position.DirectionTo(mousePosition) * 12.0f + Position;
        projectile.Velocity = Position.DirectionTo(mousePosition) * 256.0f;

        projectile.HitboxComponent.TargetGroups.Add("Enemy");

        GetParent().AddChild(projectile);
    }
}
