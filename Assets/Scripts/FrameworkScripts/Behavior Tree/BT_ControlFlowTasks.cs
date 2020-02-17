using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence<T> : BranchTask<T>
    {
        public Sequence() { }
        public Sequence(List<Task<T>> tasks) { children = tasks; }

        private int curChildIndex;

        // If a child of this sequence failed then this sequence has failed.
        public override void ChildFail(Task<T> task) { Status = BehaviorStatus.FAILED; fail(); }
        public override void ChildRunning(Task<T> task, Task<T> reporter) { }
        public override void ChildSucceed(Task<T> task)
        {
            // if true, then we've reached the end of the sequence
            if(curChildIndex++ == children.Count)
            {
                Status = BehaviorStatus.SUCCEEDED;
            }
            else
            {
                Status = BehaviorStatus.RUNNING;

                child = GetChild(curChildIndex);
            }

            Run();
        }        

        public override void Run()
        {
            switch (Status)
            {
                case BehaviorStatus.RUNNING:
                    child.Run();
                    break;

                case BehaviorStatus.FAILED:
                    fail();
                    break;

                case BehaviorStatus.SUCCEEDED:
                    success();
                    break;

                case BehaviorStatus.FRESH:
                    Start();
                    break;

                default:
                    break;
            }
        }

        public override void Start()
        {   
            curChildIndex = 0;
            child = children[curChildIndex];

            Status = BehaviorStatus.RUNNING;            
        }

        // when this task is done, it resets itself
        public override void End()
        { 
            reset();
        }
    }

    public class Selector<T> : BranchTask<T>
    {
        public Selector() { }
        public Selector(List<Task<T>> tasks) { children = tasks; }

        private int curChildIndex;

        public override void ChildFail(Task<T> task)
        {
            // if we've look at every task in children then this selector fails
            if(curChildIndex++ == children.Count)
            {
                Status = BehaviorStatus.FAILED;
            }
            else
            {
                // otherwise try the next child
                Status = BehaviorStatus.RUNNING;

                child = children[curChildIndex];
            }

            Run();
        }

        public override void ChildRunning(Task<T> task, Task<T> reporter)
        {
            
        }

        public override void ChildSucceed(Task<T> task)
        {
            Status = BehaviorStatus.SUCCEEDED;
            Run();
        }

        public override void End()
        {
            reset();
        }

        public override void Run()
        {
            switch (child.Status)
            {
                case BehaviorStatus.RUNNING:
                    child.Run();
                    break;

                case BehaviorStatus.FRESH:
                    Start();
                    break;

                case BehaviorStatus.FAILED:
                    ChildFail(child);
                    break;

                case BehaviorStatus.SUCCEEDED:
                    success();
                    break;

                default:
                    break;
            }
        }

        public override void Start()
        {
            curChildIndex = 0;
            child = children[curChildIndex];

            Status = BehaviorStatus.RUNNING;
        }
    }
}
