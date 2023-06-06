using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void PlayerChangedEventHandler(Player player);

	[Signal]
	public delegate void ScoreChangedEventHandler(int score);

	public Player Player
	{
		get => _player;
		set 
		{
			_player = value;
			
			EmitSignal(SignalName.PlayerChanged, _player);
		}
	}

	public int Score
	{
		get => _score;
		set {
			_score = value;
			
			EmitSignal(SignalName.ScoreChanged, _score);
		}
	}

	private Player _player;
	private int _score;
}
