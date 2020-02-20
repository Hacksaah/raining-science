using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum BehaviorStatus { FRESH, FAILED, SUCCEEDED, RUNNING, CANCELLED }

    public abstract class Task<T>
    {
        public Task() { }

        
        // ------ Variables -----------------------------------------
        protected BehaviorStatus status = BehaviorStatus.FRESH ;                // the current status of this task
        public BehaviorStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        protected Task<T> control;                                              // the parent of this task
        protected BehaviorTree<T> tree = null;                                  // the behavior tree this task belongs to        
        

        // --------- Functions --------------------------------------

        public void SetControl(Task<T> task) // sets this task's parent node and tree it belongs to
        {
            control = task;
            tree = task.tree;
        }

        public int AddChild(Task<T> child) // returns the index of the child task that's been added to this task's set of children (if any)
        {
            int index = AddChildToTask(child);
            return index;
        }

        protected abstract int AddChildToTask(Task<T> task);                                        // this method will add a child to the list of this task's children
        public abstract int GetChildCount();                                                        // returns the number of children for this task
        public abstract Task<T> GetChild(int i);                                                    // returns the child at given index
        public T GetObject() { if (tree != null) return tree.GetObject(); else return default; }    // returns the blackboard object from the tree this task is a part of
                        
        public abstract void Start();   // This is called once, before run (this is where you can put your condition checks), it will be assumed that this function will call Run()
        public abstract void Run();     // This is where the update logic is, this function MUST call Fail(), Running(), or Success()
        public abstract void End();     // This will be called by Success(), Fail(), Cancel()

        public abstract void ChildFail(Task<T> task);
        public abstract void ChildSucceed(Task<T> task);
        public abstract void ChildRunning(Task<T> task, Task<T> reporter);


        // ------ Control Functions -----------------------------------------------
        // Various functions that will update the status of this task and notify the parent

        // cancels this task if currently running
        public void Cancel()
        {
            CancelRunningChildren(0);
            status = BehaviorStatus.CANCELLED;
            End();
        }
        // this function is called by Cancel() and terminates all running children tasks
        protected void CancelRunningChildren(int startIndex)
        {
            for(int i = startIndex, n = GetChildCount(); i < n; i++)
            {
                Task<T> child = GetChild(i);
                if(child.status == BehaviorStatus.RUNNING) child.Reset();
            }
        }

        // resets this task to be ready next frame
        public void Reset()
        {
            if (status == BehaviorStatus.RUNNING)
            {
                Cancel();

                for (int i = 0, n = GetChildCount(); i < n; i++)
                {
                    GetChild(i).Reset();
                }
            }
            Status = BehaviorStatus.FRESH;            
        }

        // notifies the parent that this task failed
        public void Fail()
        {
            Status = BehaviorStatus.FAILED;
            End();
            control.ChildFail(this);
        }

        // notifies the parent that this task if running
        public void Running()
        {
            status = BehaviorStatus.RUNNING;
            if(control != null) control.ChildRunning(this, this);
        }
        
        // notifies the parent that this task succeeded
        public void Success()
        {            
            Status = BehaviorStatus.SUCCEEDED;
            End();
            if (control != null) control.ChildSucceed(this);
        }
    }

    // A leaf task will contain an action logic, will not have any child
    public abstract class LeafTask<T> : Task<T>
    {        
        public LeafTask() { }
        public abstract BehaviorStatus Execute();           // contains the update logic for this leaf task, it must return a Status        

        public override void Run()
        {
            BehaviorStatus result = Execute();
            switch (result)
            {
                case BehaviorStatus.RUNNING:
                    Running();
                    break;

                case BehaviorStatus.FAILED:
                    Fail();
                    break;

                case BehaviorStatus.SUCCEEDED:
                    Success();
                    break;
            }
        }

        // a leaf task won't have children, these functions will practically do nuffin
        protected override int AddChildToTask(Task<T> task) { return 0; }
        public override int GetChildCount() { return 0; }
        public override Task<T> GetChild(int i) { return null; }
        public override void ChildFail(Task<T> task) { }
        public override void ChildSucceed(Task<T> task) { }
        public override void ChildRunning(Task<T> task, Task<T> reporter) { }
    }

    // a branch task will have a set of children tasks
    public abstract class BranchTask<T> : Task<T>
    {
        protected List<Task<T>> children;

        public BranchTask() { children = new List<Task<T>>(); }         // no parameters, creates an empty child set
        public BranchTask(List<Task<T>> tasks) { children = tasks; }    // accepts a list of tasks as children

        protected override int AddChildToTask(Task<T> task)
        {
            children.Add(task);
            return children.Count - 1;
        }

        public override int GetChildCount() { return children.Count; }
            
        // returns the child at index i
        public override Task<T> GetChild(int i) { return children[i]; }        
    }

    public abstract class DecoratorTask<T> : Task<T>
    {
        protected Task<T> child = null;

        public DecoratorTask() { }
        public DecoratorTask(Task<T> _task) { child = _task; } // creates a decorator with a given task

        protected override int AddChildToTask(Task<T> task)
        {
            child = task;
            return 0;
        }

        public override int GetChildCount() { return child == null ? 0 : 1; }
        public override Task<T> GetChild(int i) { return child; }

        public override void ChildFail(Task<T> task) { Fail(); }
        public override void ChildSucceed(Task<T> task) { Success(); }
        public override void ChildRunning(Task<T> task, Task<T> reporter) { Running(); }

        public override void Run()
        {
            if (child.Status == BehaviorStatus.RUNNING) child.Run();
            else
            {
                child.SetControl(this);
                child.Start();

            }
        }

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
            
        }

        public override void ChildRunning(Task<T> task, Task<T> reporter)
        {
            Status = BehaviorStatus.RUNNING;
        }

        public override void ChildSucceed(Task<T> task)
        {
            Status = BehaviorStatus.SUCCEEDED;
        }

        public override void Start() { Status = BehaviorStatus.RUNNING; Run(); }
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

        public void notifyStatusUpdated(Task<T> task, BehaviorStatus previousStatus) { }
    }
} // namespace END