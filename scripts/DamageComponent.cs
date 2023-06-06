using Godot;
using System;

public partial class DamageComponent : Node
{
	[Signal]
	public delegate void DamagedEventHandler(HealthComponent target, int damage, Node instigator);

	[Export]
	public int Damage = 1;

	public void ApplyDamage(HealthComponent target, Node instigator)
	{
		int health = target.GetHealth();
		target.SetHealth(health - Damage, instigator);

		EmitSignal(SignalName.Damaged, target, Damage, instigator);
	}
}
