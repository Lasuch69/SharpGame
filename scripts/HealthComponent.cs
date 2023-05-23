using Godot;
using System;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void OnHealthChangedEventHandler(int health);
	
	private int _maxHealth = 1;
	private int _health = 1;
	
	[Export(PropertyHint.Range, "0,100,1,or_greater")]
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			_maxHealth = value;
		}
	}

	[Export(PropertyHint.Range, "0,100,1,or_greater")]
	public int Health
	{
		get => _health;
		set
		{
			_health = Mathf.Clamp(value, 0, _maxHealth);

			EmitSignal(SignalName.OnHealthChanged, _health);
		}
	}

	public override void _Ready()
	{
		OnHealthChanged += (health) => GD.Print(this, health);
	}
}
