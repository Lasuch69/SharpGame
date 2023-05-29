using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void PlayerChangedEventHandler(Player player);

	public Player Player
	{
		get => _player;
		set 
		{
			_player = value;

			EmitSignal(SignalName.PlayerChanged);
		}
	}

	private Player _player;
}
