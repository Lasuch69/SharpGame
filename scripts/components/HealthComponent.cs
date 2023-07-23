using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
	[Signal]
	public delegate void HealthChangedEventHandler(int newhealth, int oldHealth);

	[Signal]
	public delegate void HealthEmptyEventHandler();

	[Export]
	private int _maxHealth = 1;
	
	[Export]
	private int _health = 1;

	public void SetHealth(int health)
	{
		int oldHealth = _health;
		
		_health = Mathf.Clamp(health, 0, _maxHealth);
		
		if (_health == oldHealth)
			return;

		if (_health == 0)
			EmitSignal(SignalName.HealthEmpty);

		EmitSignal(SignalName.HealthChanged, _health, oldHealth);
	}

    public int GetHealth() => _health;
}
