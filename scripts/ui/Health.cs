using Godot;
using System;

public partial class Health : GridContainer
{
    [Export]
    public Texture2D HeartTexture;

    private HealthComponent _playerHealth;
    private Game _game;

    private Godot.Collections.Array<TextureRect> _textureRectsPool = new();

    public override void _Ready()
    {
        _game = GetNode<Game>("/root/Game");

        SetPlayer(_game.Player);

        _game.PlayerChanged += (player) => SetPlayer(player);

        SetHealth(_game.Player.HealthComponent.GetHealth());
    }

    private TextureRect CreateTextureRect()
    {
        return new TextureRect
        {
            StretchMode = TextureRect.StretchModeEnum.KeepAspect,
            Texture = HeartTexture
        };
    }

    private void SetPlayer(Player player)
    {
        if (_playerHealth != null)
            _playerHealth.HealthChanged -= OnPlayerHealthChanged;

        if (player == null)
            return;

        _playerHealth = player.HealthComponent;
        _playerHealth.HealthChanged += OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(int newHealth, int oldHealth) => SetHealth(newHealth);

    private void SetHealth(int health)
    {
        if (health > _textureRectsPool.Count)
        {
            while (health > _textureRectsPool.Count)
            {
                var textureRect = CreateTextureRect();
                AddChild(textureRect);
                _textureRectsPool.Add(textureRect);
            }
        }

        for (int i = 0; i < _textureRectsPool.Count; i++)
        {
            _textureRectsPool[i].Visible = i < health;
        }
    }
}
