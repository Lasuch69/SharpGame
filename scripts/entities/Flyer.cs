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
	NavigationAgent2D _navigationAgent;

	[Export]
	PackedScene _deathVfx = null;

	Node2D _target;

	Game _game;

	double _time = 0.0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Game");

		_target = _game.Player;
		_game.PlayerChanged += (player) => _target = player;

		HitboxComponent.TargetEntered += (target) =>
			DamageComponent.ApplyDamage(
				target.GetNode<HealthComponent>("HealthComponent"));

		HealthComponent.HealthEmpty += OnHealthEmpty;

		_navigationAgent.TargetPosition = this.Position;

		_navigationAgent.VelocityComputed += (velocity) =>
			this.Velocity = velocity;
	}

	public override void _PhysicsProcess(double delta)
	{
		_time += delta;

		if (_time > 0.2)
		{
			_time = 0.0;
			_navigationAgent.TargetPosition = _target.Position;
		}

		if (_navigationAgent.AvoidanceEnabled)
		{
			Vector2 direction = this.Position.DirectionTo(_target.Position);
			_navigationAgent.Velocity = direction * Speed;
		}
		else
		{
			Vector2 nextPathPoint = _navigationAgent.GetNextPathPosition();
			Velocity = this.Position.DirectionTo(nextPathPoint) * Speed;
		}

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
