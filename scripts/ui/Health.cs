namespace SharpGame;

public partial class Health : GridContainer
{
    [Export]
    public Texture2D HeartTexture;

    [Export]
    public Material HeartMaterial;

    HealthComponent _playerHealth;
    Game _game;

    Godot.Collections.Array<TextureRect> _textureRectsPool = new();

    public override void _Ready()
    {
        _game = GetNode<Game>("/root/Game");

        SetPlayer(_game.Player);

        _game.PlayerChanged += (player) => SetPlayer(player);

        SetHealth(_game.Player.HealthComponent.GetHealth());
    }

    TextureRect CreateTextureRect() => new()
    {
        StretchMode = TextureRect.StretchModeEnum.KeepAspect,
        Texture = HeartTexture,
        Material = HeartMaterial
    };

    void SetPlayer(Player player)
    {
        if (_playerHealth != null)
            _playerHealth.HealthChanged -= OnPlayerHealthChanged;

        if (player == null)
            return;

        _playerHealth = player.HealthComponent;
        _playerHealth.HealthChanged += OnPlayerHealthChanged;
    }

    void OnPlayerHealthChanged(int newHealth, int oldHealth) => SetHealth(newHealth);

    void SetHealth(int health)
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
