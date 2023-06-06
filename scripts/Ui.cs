using Godot;
using System;

public partial class Ui : Control
{
	[Export]
	public Label TimerLabel;

	[Export]
	public GridContainer HeartContainer;

	[Export]
	public Texture2D HeartTexture;

	[Export]
	public Label ScoreLabel;

	private ulong _timestamp;
	private HealthComponent _playerHealth;
	
	private Game _game;

	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Game");
		
		_game.PlayerChanged += (player) =>
		{
			if (_playerHealth != null)
				_playerHealth.HealthChanged -= OnPlayerHealthChanged;
			
			if (player == null)
				return;

			_playerHealth = player.GetNode<HealthComponent>("HealthComponent");
			
			_playerHealth.HealthChanged += OnPlayerHealthChanged;
			
			SetUiHearts(_playerHealth.GetHealth());
		};

		_game.ScoreChanged += OnScoreChanged;
		
		_timestamp = Time.GetTicksMsec();
	}

	public override void _Process(double delta)
	{
		TimerLabel.Text = GetTimeString(_timestamp);
	}

	private string GetTimeString(ulong timestamp)
	{ 
		int timeSeconds = (int)((Time.GetTicksMsec() - timestamp) / 1000);
		int timeMinutes = timeSeconds / 60;

		Func<int, string> Format = (time) =>
		{
			return String.Format("{0, 0:D2}", time);
		};

		return String.Format("{0}:{1}", Format(timeMinutes), Format(timeSeconds % 60));
	}

	private void SetUiHearts(int hearts)
	{
		var children = HeartContainer.GetChildren();

		foreach (Node child in children)
		{
			HeartContainer.RemoveChild(child);
		}

		for (int i = 0; hearts > i; i++)
		{
			TextureRect texture = new TextureRect();
			
			texture.StretchMode = TextureRect.StretchModeEnum.KeepAspect; 
			texture.Texture = HeartTexture;

			HeartContainer.AddChild(texture);
		}
	}

	private void SetScore(int score)
	{
		ScoreLabel.Text = String.Format("{0, 0:D3}", score);
	}

	public void OnPlayerHealthChanged(int newHealth, int oldHealth, Node instigator)
	{
		SetUiHearts(newHealth);
	}

	public void OnScoreChanged(int score)
	{
		SetScore(score);
	}
}
