namespace BehaviorTree
{
    enum BehaviorState
    {
        FAILURE = 0,
        SUCCESS = 1,
        RUNNING = 2
    }

    public abstract class BehaviorLeaf
    {
        public abstract void EnterLeaf(); 
    }

    public class BehaviorTree
    {

    }
}
