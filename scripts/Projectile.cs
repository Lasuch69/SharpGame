namespace SharpGame;

public partial class Projectile : CharacterBody2D
{
    [Export]
    public DamageComponent DamageComponent;

    [Export]
    public HitboxComponent HitboxComponent;

    GTimer _timer;

    public override void _Ready()
    {
        HitboxComponent.TargetEntered += (target) =>
        {
            HealthComponent targetHealthComponent = 
                target.GetNode<HealthComponent>("HealthComponent");

            DamageComponent.ApplyDamage(targetHealthComponent);

            QueueFree();
        };

        _timer = new GTimer(this, 3000);
        _timer.Start();
        _timer.Finished += () => QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsOnWall())
            QueueFree();

        MoveAndSlide();
    }
}
