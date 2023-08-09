namespace SharpGame;

public partial class World : Node
{
    [Export]
    public Player Player;

    [Export]
    public Ui Ui;

    [Export]
    public Spawner Spawner;

    int _score = 0;
    int _wave = 1;

    public override void _Ready()
    {
        Player.HealthComponent.HealthChanged += PlayerHealthChanged;
        Player.ScoreChanged += ScoreChanged;
        Spawner.WaveFinished += WaveFinished;

        Ui.SetHearts(Player.HealthComponent.GetHealth());
        Ui.SetScore(_score);

        Spawner.SetPlayer(Player);
        Spawner.StartWave(_wave);
    }

    void PlayerHealthChanged(int newHealth, int oldHealth)
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
