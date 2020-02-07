namespace StateMachine
{
    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public T Owner;

        public StateMachine(T o)
        {
            Owner = o;
            currentState = null;
        }

        public void ChangeState(State<T> newState)
        {
            if(currentState != null)
                currentState.ExitState(Owner);
            currentState = newState;
            currentState.EnterState(Owner);
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.UpdateState(Owner);
            }
        }

        //stops the current state
        public void HaltState()
        {
            if (currentState != null)
                currentState.ExitState(Owner);
            currentState = null;
        }
    }

    public abstract class State<T>
    {
        public abstract void EnterState(T owner);
        public abstract void ExitState(T owner);
        public abstract void UpdateState(T owner);
    }
}
