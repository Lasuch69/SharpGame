using Godot;
using System;

public partial class HitboxComponent : Area2D
{
    [Signal]
    public delegate void TargetEnteredEventHandler(Node2D target);

    [Signal]
    public delegate void TargetExitedEventHandler(Node2D target);

    [Export]
    public Godot.Collections.Array<String> TargetGroups = new();

    public Godot.Collections.Array<Node2D> Targets = new();

    public override void _Ready()
    {
        this.BodyEntered += (Node2D body) =>
        {
            bool hasGroup = false;

            foreach (String group in body.GetGroups())
            {
                if (TargetGroups.IndexOf(group) == -1)
                    continue;

                hasGroup = true;
                break;
            }

            if (!hasGroup)
                return;

            Targets.Add(body);

            EmitSignal(SignalName.TargetEntered, body);
        };

        this.BodyExited += (Node2D body) =>
        {
            bool removed = Targets.Remove(body);

            if (!removed)
                return;

            EmitSignal(SignalName.TargetExited, body);
        };
    }
}
