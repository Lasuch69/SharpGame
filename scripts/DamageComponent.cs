using Godot;
using System;

public partial class DamageComponent : Node
{
	[Signal]
	public delegate void DamagedEventHandler(HealthComponent target, int damage);

	[Export]
	public int Damage
	{
		get => _damage;
		set
		{
			_damage = value;
		}
	}

	private int _damage = 1;

	public void ApplyDamage(HealthComponent target)
	{
		target.Health -= _damage;

		EmitSignal(SignalName.Damaged, target, _damage);
	}
}
