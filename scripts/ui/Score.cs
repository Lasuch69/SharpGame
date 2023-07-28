namespace SharpGame;

public partial class Score : Label
{
    private Game _game;

    public override void _Ready()
    {
        _game = GetNode<Game>("/root/Game");

        _game.ScoreChanged += (score) =>
        {
            Text = String.Format("{0, 0:D3}", score);
        };
    }
}
