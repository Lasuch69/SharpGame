namespace SharpGame;

[GlobalClass]
public partial class Flyer : CharacterBody2D
{
    [Export]
    public float Speed = 80.0f;

    [Export]
    public int ScoreOnKill = 100;

    [Export]
    public Area2D Area2D;

    [Export]
    public DamageComponent DamageComponent;

    [Export]
    public HealthComponent HealthComponent;

    [Export]
    public NavigationAgent2D NavigationAgent;

    [Export]
    PackedScene _deathVfx;

    Player _player;

    Game _game;

    double _time = 0.0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _game = GetNode<Game>("/root/Game");

        _player = _game.Player;
        _game.PlayerChanged += (player) => _player = player;

        Area2D.BodyEntered += OnBodyEntered;
        HealthComponent.HealthEmpty += OnHealthEmpty;

        NavigationAgent.TargetPosition = Position;

        NavigationAgent.VelocityComputed += (velocity) => Velocity = velocity;
    }

    public override void _PhysicsProcess(double delta)
    {
        _time += delta;

        if (_time > 0.2)
        {
            _time = 0.0;
            NavigationAgent.TargetPosition = _player.Position;
        }

        if (NavigationAgent.AvoidanceEnabled)
        {
            Vector2 direction = Position.DirectionTo(_player.Position);
            NavigationAgent.Velocity = direction * Speed;
        }
        else
        {
            Vector2 nextPathPoint = NavigationAgent.GetNextPathPosition();
            Velocity = Position.DirectionTo(nextPathPoint) * Speed;
        }

        MoveAndSlide();
    }

    void OnBodyEntered(Node2D body)
    {
        if (body is not Player)
            return;

        Player player = (Player)body;
        DamageComponent.ApplyDamage(player.HealthComponent);
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
