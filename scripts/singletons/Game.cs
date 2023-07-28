global using Godot;
global using System;
global using System.Linq;

namespace SharpGame;

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
            if (_player == value)
                return;

            _player = value;
            EmitSignal(SignalName.PlayerChanged, _player);
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            if (_score == value)
                return;

            _score = value;
            EmitSignal(SignalName.ScoreChanged, _score);
        }
    }

    private int _score = 0;
    private Player _player = null;
}
