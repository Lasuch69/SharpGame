public partial class Projectile : CharacterBody2D
{
    [Export]
    public DamageComponent DamageComponent;

    [Export]
    public HitboxComponent HitboxComponent;

    [Export]
    public Timer Timer;

    public override void _Ready()
    {
        HitboxComponent.TargetEntered += (target) =>
        {
            HealthComponent targetHealthComponent = target.GetNode<HealthComponent>("HealthComponent");
            DamageComponent.ApplyDamage(targetHealthComponent);

            QueueFree();
        };

        Timer.Timeout += () => QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsOnWall())
            QueueFree();

        MoveAndSlide();
    }
}
