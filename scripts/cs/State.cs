namespace SharpGame;

[GlobalClass]
public partial class State : Node
{
	public CharacterBody2D CharacterBody2D;

	public virtual void Enter() { }

	public virtual State PhysicsProcess(double delta) => null;

	public virtual State Process(double delta) => null;

	public virtual State Input(InputEvent @event) => null;
}
