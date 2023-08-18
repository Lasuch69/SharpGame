namespace SharpGame;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Signal]
    public delegate void HealthChangedEventHandler(int oldHealth, int newHealth);

    [Signal]
    public delegate void HealthEmptyEventHandler();

    [Export]
    int _maxHealth = 1;

    [Export]
    int _health = 1;

    public void Damage(int health)
    {
        SetHealth(GetHealth() - health);
    }

    public void Heal(int health)
    {
        SetHealth(GetHealth() + health);
    }

    public void SetHealth(int health)
    {
        int oldHealth = _health;
        _health = Mathf.Clamp(health, 0, _maxHealth);

        EmitSignal(SignalName.HealthChanged, oldHealth, _health);

        if (_health <= 0)
            EmitSignal(SignalName.HealthEmpty);
    }

    public int GetHealth()
    {
        return _health;
    }
}
