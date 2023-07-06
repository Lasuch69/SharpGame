using Godot;
using System;

public partial class Health : GridContainer
{
	[Export]
	public Texture2D HeartTexture;

	private HealthComponent _playerHealth;	
	private Game _game;

	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Game");
		
		SetPlayer(_game.Player);

		_game.PlayerChanged += (player) => SetPlayer(player);
	}
 
	private void OnPlayerHealthChanged(int newHealth, int oldHealth, Node instigator)
	{
		SetUiHearts(newHealth);
	}
	
	private void SetPlayer(Player player)
	{
		if (_playerHealth != null)
			_playerHealth.HealthChanged -= OnPlayerHealthChanged;

		if (player == null)
			return;

		_playerHealth = player.HealthComponent;

		_playerHealth.HealthChanged += OnPlayerHealthChanged;
		
		SetUiHearts(_playerHealth.GetHealth());
	}

	private void SetUiHearts(int hearts)
	{
		var children = GetChildren();

		foreach (Node child in children)
		{
			RemoveChild(child);
		}

		for (int i = 0; hearts > i; i++)
		{
			TextureRect texture = new TextureRect();
			
			texture.StretchMode = TextureRect.StretchModeEnum.KeepAspect; 
			texture.Texture = HeartTexture;

			AddChild(texture);
		}
	}
}
