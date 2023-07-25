using Godot;
using System;

[GlobalClass]
public partial class NavigationDebug : Node2D
{
    private NavigationComponent _navigationComponent;

    public override void _Ready() => _navigationComponent = GetParent<NavigationComponent>();

    public override void _Draw()
    {
        var path = _navigationComponent.Path;

        if (path.Count < 2)
            return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            DrawLine(path[i], path[i + 1], Color.Color8(0, 255, 0, 255));
        }
    }

    public override void _Process(double delta) => QueueRedraw();
}
