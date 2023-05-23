using Godot;
using System;

public partial class ImpulseArea : Area2D
{
	[Export]
	private Timer _timer;

	private Player _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		_timer.Timeout += _OnTimerTimeout;
		BodyEntered += _OnBodyEntered;
		BodyExited += _OnBodyExited;
	}
	
	public void ApplyDamage()
	{
		HealthComponent healthComponent = _player.GetNode<HealthComponent>("HealthComponent");
		
		_player.Velocity = (Vector2.Up * 150.0f);
		healthComponent.Health -= 1;
	}

	public void _OnTimerTimeout()
	{
		ApplyDamage();
	}

	public void _OnBodyEntered(Node2D body)
	{
		if (!(body is Player))
			return;

		_player = (Player)body;

		ApplyDamage();

		_timer.Start();
	}

	public void _OnBodyExited(Node2D body)
	{
		if (!(body is Player))
			return;

		_player = null;

		_timer.Stop();
	}
}
