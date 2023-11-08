namespace SharpGame;

public partial class GameUI : Control
{
	[Export]
	HeartContainer _heartContainer;

	[Export]
	Label _scoreLabel;

	public void SetHearts(int count)
	{
		_heartContainer.SetHearts(count);
	}

	public void SetScore(int score)
	{
		_scoreLabel.Text = String.Format("{0, 0:D3}", score);
	}
}
