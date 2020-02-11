using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum BehaviorState
    {
        FAILURE = 0,
        SUCCESS = 1,
        RUNNING = 2
    }

    public abstract class BehaviorNode
    {
        public BehaviorNode parentNode;
        protected BehaviorState currentNodeState;
        public BehaviorState nodeState { get { return currentNodeState; } }

        public BehaviorNode() { }
        public abstract BehaviorState Process();
    }

    public class SelectorNode : BehaviorNode
    {
        protected List<BehaviorNode> nodes = new List<BehaviorNode>();

        SelectorNode(BehaviorNode _parentNode, List<BehaviorNode> _nodes)
        {
            parentNode = _parentNode;
            nodes = _nodes;
        }

        public override BehaviorState Process()
        {
            foreach (BehaviorNode node in nodes)
            {
                switch (node.Process())
                {
                    case BehaviorState.FAILURE:
                        continue;

                    case BehaviorState.SUCCESS:
                        currentNodeState = BehaviorState.SUCCESS;
                        return currentNodeState;

                    case BehaviorState.RUNNING:
                        currentNodeState = BehaviorState.RUNNING;
                        return currentNodeState;

                    default:
                        continue;
                }
            }
            currentNodeState = BehaviorState.FAILURE;
            return currentNodeState;
        }
    }

    public class SequenceNode : BehaviorNode
    {
        private List<BehaviorNode> nodes = new List<BehaviorNode>();
        private int sequenceIndex = 0;

        SequenceNode(List<BehaviorNode> _nodes)
        {
            nodes = _nodes;
        }

        public override BehaviorState Process()
        {
            BehaviorNode currNode = nodes[sequenceIndex];
            switch(currNode.Process())
            {
                case BehaviorState.FAILURE:
                    currentNodeState = BehaviorState.FAILURE;
                    return currentNodeState;

                case BehaviorState.SUCCESS:
                    sequenceIndex++;
                    currentNodeState = BehaviorState.RUNNING;
                    if (sequenceIndex == nodes.Count)
                    {
                        currentNodeState = BehaviorState.SUCCESS;
                        sequenceIndex = 0;
                    }
                    return currentNodeState;

                case BehaviorState.RUNNING:
                    currentNodeState = BehaviorState.RUNNING;
                    return currentNodeState;

                default:
                    currentNodeState = BehaviorState.SUCCESS;
                    return currentNodeState;
            }
        }
    }

    public class BehaviorTree
    {
        private BehaviorNode root;
        private BehaviorNode currBehaviorNode;

        BehaviorTree(BehaviorNode _rootNode)
        {
            root = _rootNode;
            currBehaviorNode = null;
        }

        public void Update()
        {
            if (currBehaviorNode != null)
                currBehaviorNode.Process();
            else
                root.Process();
        }
    }
}
