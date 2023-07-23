using Godot;
using System;

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
	private PackedScene _deathVfx = null;

	public CollisionObject2D Target;
	
	private Game _game;
	
	private PhysicsDirectSpaceState2D _spaceState;
	
	private double _time = 0.0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Game"); 

		Target = _game.Player;
		_game.PlayerChanged += (player) => Target = player;

		HitboxComponent.TargetEntered += (target) => DamageComponent.ApplyDamage(target.GetNode<HealthComponent>("HealthComponent"));

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

		var parameters = new PhysicsRayQueryParameters2D();

		parameters.From = Position;
		parameters.To = Target.Position;
		parameters.CollisionMask = 0b1;
		parameters.Exclude = new Godot.Collections.Array<Rid>{Target.GetRid()};

		var result = _spaceState.IntersectRay(parameters);

		if (result.Count == 0)
			if (Position.DistanceTo(Target.Position) > 1.0f)
				Velocity = Position.DirectionTo(Target.Position) * Speed;
			else
				Velocity = new ();
		else
			Velocity = NavigationComponent.GetDirection(Position) * Speed;

		MoveAndSlide();
	}

	private void OnHealthEmpty()
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
