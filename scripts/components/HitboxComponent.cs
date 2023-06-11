using Godot;
using System;
using System.Collections.Generic;

public partial class HitboxComponent : Area2D
{
	[Signal]
	public delegate void TargetEnteredEventHandler(Node2D target);

	[Signal]
	public delegate void TargetExitedEventHandler(Node2D target);

	[Export]
	public String TargetGroup;

	public List<Node2D> Targets = new();

	public override void _Ready()
	{
		this.BodyEntered += (Node2D body) => 
		{ 
			if (!body.IsInGroup(TargetGroup))
				return;
			
			Targets.Add(body);

			EmitSignal(SignalName.TargetEntered, body); 
		};
		
		this.BodyExited += (Node2D body) => 
		{ 
			if (!body.IsInGroup(TargetGroup))
				return;
			
			Targets.Remove(body);

			EmitSignal(SignalName.TargetExited, body);
		};
	}
}
