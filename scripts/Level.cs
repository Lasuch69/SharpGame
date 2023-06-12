using Godot;
using System;

public partial class Level : Node2D
{
	[Export]
	public NavigationRegion2D NavigationMap;

	public override void _Ready()
	{
		GetNode<Game>("/root/Game").SetLevel(this);
	}
}
