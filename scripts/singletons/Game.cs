using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	[Signal]
	public delegate void PlayerChangedEventHandler(Player player);

	[Signal]
	public delegate void ScoreChangedEventHandler(int score);

	[Signal]
	public delegate void WaveStartedEventHandler(int wave);

	[Signal]
	public delegate void WaveFinishedEventHandler(int wave);

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
		set
		{
			_score = value;
			
			EmitSignal(SignalName.ScoreChanged, _score);
		}
	}

	public int Wave
	{
		get => _wave;
		set => GD.PushError("Can't set Wave property.");
	}

	public List<PackedScene> SpawnQueue = new ();
	
	private Player _player = null;
	private int _score = 0;
	private int _wave = 0;

	private PackedScene _enemyScene = (PackedScene)GD.Load("res://scenes/flyer.tscn");

	public void StartWave()
	{
		_wave++;

		for (int i = 0; i < _wave * 2; i++)
		{
			SpawnQueue.Add(_enemyScene);
		}

		GD.Print("Wave: ", _wave);
		EmitSignal(SignalName.WaveStarted, _wave);
	}

	public void FinishWave()
	{
		GD.Print("Wave finished!");
		EmitSignal(SignalName.WaveFinished, _wave);
	}
}
