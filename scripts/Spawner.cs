using Godot;
using System;

public partial class Spawner : Node2D
{
    [Signal]
    public delegate void WaveFinishedEventHandler();

    [Export]
    private Player _player = null;

    [Export]
    private Timer _timer = null;

    private PhysicsDirectSpaceState2D _spaceState = null;
    private PackedScene _enemyScene = (PackedScene)GD.Load("res://scenes/flyer.tscn");
    private Random _random = new();

    private Godot.Collections.Array<PackedScene> _queue = new();
    private Godot.Collections.Array<Node2D> _entities = new();

    public override void _Ready() => _timer.Timeout += OnTimerTimeout;

    public override void _PhysicsProcess(double delta)
    {
        _spaceState ??= PhysicsServer2D.SpaceGetDirectState(GetWorld2D().Space);
    }

    private void OnTimerTimeout()
    {
        if (_queue.Count == 0)
        {
            _timer.Stop();
            return;
        }

        int last = _queue.Count - 1;
        PackedScene scene = _queue[last];
        _queue.RemoveAt(last);

        Node2D entity = Spawn(scene);
        _entities.Add(entity);

        entity.TreeExiting += () =>
        {
            _entities.Remove(entity);

            if (_entities.Count == 0 && _queue.Count == 0)
                EmitSignal(SignalName.WaveFinished);
        };
    }

    public void StartWave(int wave)
    {
        int points = Mathf.Clamp(wave, 1, int.MaxValue);

        _queue.Resize(points);
        _queue.Fill(_enemyScene);

        _timer.Start();
    }

    private Vector2 GetRandomPoint(float desiredDistance)
    {
        float a = _random.NextSingle() * 2 * MathF.PI;

        float i = _random.NextSingle() * 0.25f;
        i -= i / 2.0f;

        float r = desiredDistance * (1.0f + i);

        return new Vector2(r * MathF.Cos(a), r * MathF.Sin(a));
    }

    private Node2D Spawn(PackedScene scene)
    {
        Node instance = scene.Instantiate();

        if (!instance.IsClass("Node2D"))
            return null;

        if (_player == null)
            return null;

        Vector2 position = Vector2.Zero;

        int i = 0;

        while (i < 32)
        {
            Vector2 point = GetRandomPoint(128.0f) + _player.Position;

            PhysicsRayQueryParameters2D parameters = new()
            {
                From = _player.Position,
                To = point
            };
            parameters.Exclude.Add(_player.GetRid());

            if (_spaceState.IntersectRay(parameters).Count == 0)
            {
                position = point;
                break;
            }

            i++;
        }

        Node2D entity = (Node2D)instance;

        entity.Position = position;

        GetParent().AddChild(entity);

        return entity;
    }
}
