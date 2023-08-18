namespace SharpGame;

public partial class World : Node
{
    [Export]
    public Player Player;

    [Export]
    public Ui Ui;

    [Export]
    public Spawner Spawner;

    [Export]
    AnimationPlayer _animationPlayer;

    int _score = 0;
    int _wave = 1;

    public override void _Ready()
    {
        Player.HealthComponent.HealthEmpty += PlayerHealthEmpty;
        Player.HealthComponent.HealthChanged += PlayerHealthChanged;
        Player.ScoreChanged += ScoreChanged;
        Spawner.WaveFinished += WaveFinished;

        Ui.SetHearts(Player.HealthComponent.GetHealth());
        Ui.SetScore(_score);

        Spawner.SetPlayer(Player);
        Spawner.StartWave(_wave);
    }

    void PlayerHealthEmpty()
    {
        _animationPlayer.Play("root_anim/game_over");
    }

    void PlayerHealthChanged(int oldHealth, int newHealth)
    {
        Ui.SetHearts(newHealth);
    }

    void ScoreChanged(int score)
    {
        _score += score;
        Ui.SetScore(_score);
    }

    void WaveFinished()
    {
        _wave++;
        Spawner.StartWave(_wave);
    }
}
