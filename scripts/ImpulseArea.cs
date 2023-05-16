using Godot;
using System;

public partial class ImpulseArea : Area2D
{
	[Export]
	private CharacterBody2D character;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += _OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			float direction = Vector2.Zero.DirectionTo(new Vector2(character.Velocity.X, 0.0f)).X;
			player.Velocity = (Vector2.Up * 100.0f) + ((Vector2.Right * direction) * 150.0f);
			GD.Print(direction);
		}
	}
}
