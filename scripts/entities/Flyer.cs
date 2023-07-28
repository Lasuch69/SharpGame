namespace SharpGame;

[GlobalClass]
public partial class Flyer : CharacterBody2D
{
    [Export]
    public float Speed = 80.0f;

    [Export]
    public int ScoreOnKill = 100;

    [Export]
    public DamageComponent DamageComponent;

    [Export]
    public HealthComponent HealthComponent;

    [Export]
    public HitboxComponent HitboxComponent;

    [Export]
    public NavigationComponent NavigationComponent;

    [Export]
    PackedScene _deathVfx = null;

    public CollisionObject2D Target;

    Game _game;

    PhysicsDirectSpaceState2D _spaceState;

    double _time = 0.0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _game = GetNode<Game>("/root/Game");

        Target = _game.Player;
        _game.PlayerChanged += (player) => Target = player;

        HitboxComponent.TargetEntered += (target) => 
            DamageComponent.ApplyDamage(
                target.GetNode<HealthComponent>("HealthComponent"));

        HealthComponent.HealthEmpty += OnHealthEmpty;
    }

    public override void _PhysicsProcess(double delta)
    {
        _spaceState ??= PhysicsServer2D.SpaceGetDirectState(GetWorld2D().Space);

        _time += delta;

        if (_time > 0.25)
        {
            _time = 0.0;
            NavigationComponent.GeneratePath(Position, Target.Position);
        }

        var parameters = new PhysicsRayQueryParameters2D
        {
            From = Position,
            To = Target.Position,
            CollisionMask = 0b1,
            Exclude = new Godot.Collections.Array<Rid> { Target.GetRid() }
        };

        var result = _spaceState.IntersectRay(parameters);

        if (result.Count == 0)
            if (Position.DistanceTo(Target.Position) > 1.0f)
                Velocity = Position.DirectionTo(Target.Position) * Speed;
            else
                Velocity = new();
        else
            Velocity = NavigationComponent.GetDirection(Position) * Speed;

        MoveAndSlide();
    }

    void OnHealthEmpty()
    {
        _game.Score += ScoreOnKill;

        if (_deathVfx != null)
        {
            Node2D instance = (Node2D)_deathVfx.Instantiate();
            instance.Position = Position;

            GetParent().AddChild(instance);
        }

        QueueFree();
    }
}
