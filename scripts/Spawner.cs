using System.Collections.Generic;

namespace SharpGame;

public partial class Spawner : Node2D
{
    [Signal]
    public delegate void WaveFinishedEventHandler();

    [Export]
    Timer _timer;

    [Export]
    PackedScene _enemy;

    Player _player;

    List<PackedScene> _queue = new();
    List<Node2D> _entities = new();

    Random _random = new();

    PhysicsDirectSpaceState2D _spaceState;

    public override void _Ready()
    {
        _timer.Timeout += OnTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        _spaceState ??= PhysicsServer2D.SpaceGetDirectState(GetWorld2D().Space);
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void StartWave(int wave)
    {
        int points = Mathf.Clamp(wave, 1, int.MaxValue);

        while (points > 0)
        {
            _queue.Add(_enemy);
            points -= 1;
        }

        _timer.Start();
    }

    Vector2 GetRandomPoint(float desiredDistance)
    {
        float a = _random.NextSingle() * 2 * MathF.PI;

        float i = _random.NextSingle() * 0.25f;
        i -= i / 2.0f;

        float r = desiredDistance * (1.0f + i);

        return new Vector2(r * MathF.Cos(a), r * MathF.Sin(a));
    }

    Node2D Spawn(PackedScene scene)
    {
        if (_player == null)
            return null;

        Flyer entity = (Flyer)scene.Instantiate();

        entity.SetPlayer(_player);

        Vector2 position = Vector2.Zero;

        for (int i = 0; i < 32; i++)
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
        }

        entity.Position = position;
        AddChild(entity);

        return entity;
    }

    void OnTimerTimeout()
    {
        if (_queue.Count == 0)
        {
            _timer.Stop();
            return;
        }

        int idx = _queue.Count - 1;
        PackedScene scene = _queue[idx];
        _queue.RemoveAt(idx);

        Node2D entity = Spawn(scene);
        _entities.Add(entity);

        entity.TreeExiting += () => EntityKilled(entity);
    }

    void EntityKilled(Node2D entity)
    {
        _entities.Remove(entity);

        if (_entities.Count == 0 && _queue.Count == 0)
            EmitSignal(SignalName.WaveFinished);
    }
}
