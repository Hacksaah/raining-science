using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum BehaviorStatus { CANCELLED, FAILED, SUCCEEDED, RUNNING, FRESH }

    public abstract class Task<T>
    {
        public Task() { }        
        public BehaviorStatus Status { get; set; }      // the current status of this task
        public Task<T> Control { get; set; }            // the parent of this task        
        protected Task<T> child = null;                 // the child of this task
        protected BehaviorTree<T> tree = null;          // the behavior tree this task belongs to
        protected T Owner;                              // the blackboard object being manipulated for this task


        public abstract void Start();   // This is called once, before run (this is where you can put your condition checks)
        public abstract void Run();     // This is where the update logic is, this function MUST call Fail(), Running(), or Success()
        public abstract void End();     // This will be called by Succeed(), Fail(), Cancel()

        public abstract void ChildFail(Task<T> task);
        public abstract void ChildSucceed(Task<T> task);
        public abstract void ChildRunning(Task<T> task, Task<T> reporter); // called when an ancestor needs

        public void SetChildTask(Task<T> task) { child = task; }
        public Task<T> GetChildTask() { return child; }
        public void SetTree(BehaviorTree<T> Tree) { tree = Tree; }
        public void SetOwner(T owner) { Owner = owner; }
        public T GetOwner() { return Owner; }

        
        // Various functions that will notify the parent of this tasks status OR refresh it's status
        public void cancel() { if(Status == BehaviorStatus.RUNNING) { Status = BehaviorStatus.CANCELLED; End(); } } // cancels this task if currently running        
        public void reset() { Status = BehaviorStatus.FRESH; }                                                      // resets this task to be ready next frame
        public void fail() { End(); Control.ChildFail(this); }                                                      // notifies the parent that this task failed
        public void running() { Control.ChildRunning(this, this); }                                                 // notifies the parent that this task if running
        public void success() { End(); Control.ChildSucceed(this); }                                                // notifies the parent that this task succeeded
    }

    // A leaf task will contain an action logic, will not have any child
    public abstract class LeafTask<T> : Task<T>
    {        
        public LeafTask() { }
        public abstract BehaviorStatus Execute();           // contains the update logic for this leaf task, it must return a Status        

        public override void Run()
        {
            switch (Status)
            {
                case BehaviorStatus.FRESH:
                    Start();
                    break;

                case BehaviorStatus.RUNNING:
                    Status = Execute();
                    running();
                    break;

                case BehaviorStatus.FAILED:
                    fail();
                    break;

                case BehaviorStatus.SUCCEEDED:
                    success();
                    break;
            }
        }

        // assumes a leaf node has no children, therefore these functions should do nothing
        public override void ChildFail(Task<T> task) { }
        public override void ChildSucceed(Task<T> task) { }
        public override void ChildRunning(Task<T> task, Task<T> reporter) { }
        private new void SetChildTask(Task<T> task) { }
    }

    public abstract class BranchTask<T> : Task<T>
    {
        protected List<Task<T>> children;

        public BranchTask() { children = new List<Task<T>>(); }         // no parameters, creates an empty child set
        public BranchTask(List<Task<T>> tasks) { children = tasks; }    // accepts a list of tasks as children

        public void AddTaskToChildren(Task<T> child) { children.Add(child); }   // adds child to this task's list of children
        public int GetChildrenCount() { return children.Count; }

        // returns the child at index i
        public Task<T> GetChild(int i)
        {
            if (i >= 0 && i < children.Count) return children[i];
            else return null;
        }

        public new void reset()
        {
            base.reset();
            foreach(Task<T> child in children)
            {
                child.reset();
            }
        }
    }

    public abstract class DecoratorTask<T> : Task<T>
    {
        public DecoratorTask() { }
        public DecoratorTask(Task<T> _task) { child = _task; } // creates a decorator with a given task

        public override void ChildFail(Task<T> task) { Control.ChildFail(task); }
        public override void ChildSucceed(Task<T> task) { Control.ChildSucceed(task); }
        public override void ChildRunning(Task<T> task, Task<T> reporter) { Control.ChildRunning(task, this); }

        public new void reset()
        {
            base.reset();
            child.reset();
        }
    }

    public class BehaviorTree<T> : Task<T>
    {
        public BehaviorTree(Task<T> rootTask, T bbObject)
        {
            Owner = bbObject;
            child = rootTask;
            
        }
       
        public override void ChildFail(Task<T> task)
        {
            Status = BehaviorStatus.FAILED;
            Run();
        }

        public override void ChildRunning(Task<T> task, Task<T> reporter)
        {
            Status = BehaviorStatus.RUNNING;
            Run();
        }

        public override void ChildSucceed(Task<T> task)
        {
            Status = BehaviorStatus.SUCCEEDED;
            Run();
        }

        public override void Start() { Status = BehaviorStatus.RUNNING; }
        public override void End() { }

        public override void Run()
        {
            switch (Status)
            {
                case BehaviorStatus.FRESH:
                    Start();
                    break;

                case BehaviorStatus.FAILED:
                    reset();
                    break;

                case BehaviorStatus.SUCCEEDED:
                    reset();
                    break;

                case BehaviorStatus.RUNNING:
                    child.Run();
                    break;

            }
        }        
    }
}