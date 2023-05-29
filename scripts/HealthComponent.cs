using Godot;
using System;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void HealthChangedEventHandler(int newhealth, int oldHealth);

	[Export(PropertyHint.Range, "0,100,1,or_greater")]
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			_maxHealth = value;

			// clamp to new _maxHealth
			Health = Health;
		}
	}

	[Export(PropertyHint.Range, "0,100,1,or_greater")]
	public int Health
	{
		get => _health;
		set
		{
			int newHealth = Mathf.Clamp(value, 0, _maxHealth);

			if (_health != newHealth)
				EmitSignal(SignalName.HealthChanged, newHealth, _health);

			_health = newHealth;
		}
	}

	private int _maxHealth = 1;
	private int _health = 1;
}
