using Godot;
using System;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void HealthChangedEventHandler(int newhealth, int oldHealth, Node instigator);

	[Export]
	private int _maxHealth = 1;
	
	[Export]
	private int _health = 1;

	public void SetHealth(int health, Node instigator)
	{
		int oldHealth = _health;
		
		_health = Mathf.Clamp(health, 0, _maxHealth);
		
		if (_health != oldHealth)
			EmitSignal(SignalName.HealthChanged, _health, oldHealth, instigator);
	}

	public int GetHealth()
	{
		return _health;
	}
}
