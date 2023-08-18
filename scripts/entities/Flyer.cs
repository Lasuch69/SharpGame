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
    public HealthComponent HealthComponent;

    [Export]
    public NavigationAgent2D NavigationAgent;

    Player _player;

    double _time = 0.0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
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

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    void OnBodyEntered(Node2D body)
    {
        if (body is not Player)
            return;

        Player player = (Player)body;
        player.HealthComponent.Damage(1);
    }

    void OnHealthEmpty()
    {
        PackedScene effectPackedScene = (PackedScene)GD.Load("res://scenes/visual_effects/explosion/explosion.tscn");

        Node2D effectInstance = (Node2D)effectPackedScene.Instantiate();

        effectInstance.Position = Position;

        GetParent().AddChild(effectInstance);
        effectInstance.Call("start");

        QueueFree();
    }
}
