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
			
			SetUiHearts(_playerHealth.Health);
		};
		
		_timestamp = Time.GetTicksMsec();
	}

	public override void _Process(double delta)
	{
		TimerLabel.Text = GetTimeString(_timestamp);
	}

	private string GetTimeString(ulong timestamp)
	{ 
		int timeSeconds = (int)((Time.GetTicksMsec() - timestamp) / 1000);
	   
		Func<int, string> toString = (time) =>
		{
			if (time < 10)
				return $"0{time}";

			return $"{time}";
		};

		return $"{toString(timeSeconds / 60)}:{toString(timeSeconds % 60)}";
	}

	private void SetUiHearts(int amount)
	{
		var children = HeartContainer.GetChildren();

		foreach (Node child in children)
		{
			HeartContainer.RemoveChild(child);
		}

		for (int i = 0; amount > i; i++)
		{
			TextureRect texture = new TextureRect();
			
			texture.StretchMode = TextureRect.StretchModeEnum.KeepAspect; 
			texture.Texture = HeartTexture;

			HeartContainer.AddChild(texture);
		}
	}

	public void OnPlayerHealthChanged(int newHealth, int oldHealth)
	{
		SetUiHearts(newHealth);
	}
}
