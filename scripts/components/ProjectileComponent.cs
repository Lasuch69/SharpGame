namespace SharpGame;

[GlobalClass]
public partial class ProjectileComponent : Node
{
    [Signal]
    public delegate void OnCollisionEventHandler
        (Vector2 position, Godot.Collections.Dictionary result);

    Node _wrapper;

    public override void _Ready()
    {
        _wrapper = GetNode<Node>("ProjectileComponentWrapper");
        _wrapper.Set("_projectile_component", this);
    }

    public void SpawnProjectile(Vector2 position, Vector2 velocity, int collisionMask)
    {
        _wrapper.Call("_spawn_projectile", position, velocity, collisionMask);
    }

    void OnCollisionCallback(Vector2 position, Godot.Collections.Dictionary result)
    {
        EmitSignal(SignalName.OnCollision, position, result);
    }
}
