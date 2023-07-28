public partial class World : Node
{
    [Signal]
    public delegate void WaveStartedEventHandler(int wave);

    public int Wave { get => _wave; }

    private int _wave = 1;

    [Export]
    private Spawner _spawner = null;

    public override void _Ready()
    {
        _spawner.StartWave(_wave);

        _spawner.WaveFinished += () =>
        {
            _wave++;
            _spawner.StartWave(_wave);
        };
    }
}
