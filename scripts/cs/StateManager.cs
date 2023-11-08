namespace SharpGame;

[GlobalClass]
public partial class StateManager : Node
{
	[Export]
	public CharacterBody2D CharacterBody2D;

	[Export]
	public State StartState;

	public State CurrentState = null;

	public override void _Ready()
	{
		var children = GetChildren();

		foreach (State child in children)
		{
			child.CharacterBody2D = CharacterBody2D;
		}

		ChangeState(StartState);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (CurrentState == null)
			return;

		State state = CurrentState.PhysicsProcess(delta);
		ChangeState(state);
	}

	public override void _Process(double delta)
	{
		if (CurrentState == null)
			return;

		State state = CurrentState.Process(delta);
		ChangeState(state);
	}

	public override void _Input(InputEvent @event)
	{
		if (CurrentState == null)
			return;

		State state = CurrentState.Input(@event);
		ChangeState(state);
	}

	void ChangeState(State newState)
	{
		if (newState == null)
			return;

		CurrentState = newState;
		CurrentState.Enter();
	}
}
