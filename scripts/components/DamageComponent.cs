namespace SharpGame;

[GlobalClass]
public partial class DamageComponent : Node
{
    [Signal]
    public delegate void DamagedEventHandler(HealthComponent target, int damage);

    [Export]
    public int Damage = 1;

    public void ApplyDamage(HealthComponent target)
    {
        int health = target.GetHealth();
        target.SetHealth(health - Damage);

        EmitSignal(SignalName.Damaged, target, Damage);
    }
}
