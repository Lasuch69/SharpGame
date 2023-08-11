namespace SharpGame;

[GlobalClass]
public partial class StateManager : Node
{
    [Export]
    public Player Entity;

    [Export]
    public State StartState;

    public State CurrentState = null;

    public override void _Ready()
    {
        Dead dead = new Dead();
        AddChild(dead);
        dead.Name = "Dead";
        Entity.HealthComponent.HealthEmpty += () => ChangeState(dead);

        var children = GetChildren();

        foreach (State child in children)
        {
            child.Entity = Entity;
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
