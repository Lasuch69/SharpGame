using Godot;
using System;

[GlobalClass]
public partial class NavigationComponent : Node
{
    public Godot.Collections.Array<Vector2> Path { get => _path; }

    private Navigation _navigation;
    private Godot.Collections.Array<Vector2> _path = new();

    public override void _Ready() => _navigation = GetNode<Navigation>("/root/Navigation");

    public void GeneratePath(Vector2 from, Vector2 to) => _path = _navigation.GetPath(from, to);

    public Vector2 GetDirection(Vector2 from, float minPointDistance = 0.005f)
    {
        Vector2 direction = new();

        // Return null if path does not exist or is less than 2
        if (_path == null || _path.Count < 2)
            return direction;

        direction = from.DirectionTo(_path[1]);

        if (from.DistanceTo(_path[0]) < minPointDistance)
            _path.RemoveAt(0);

        return direction;
    }
}
