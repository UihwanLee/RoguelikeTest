using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING,
    UNDEF,
}

public abstract class Node
{
    public NodeState state = NodeState.UNDEF;
    public string name;
    public bool hasCondition;
    public List<Node> children = new List<Node>();

    public virtual void AddChild(Node node)
    {
        children.Add(node);
    }

    public virtual void TagCondition()
    {
        foreach(var child in children)
        {
            child.TagCondition();
            if (child.hasCondition)
                hasCondition = true;
        }
    }

    public void Reset()
    {
        state = NodeState.UNDEF;
        foreach(var child in children)
        {
            child.Reset();
        }
    }

    public abstract NodeState Run();
}

/// <summary>
/// Sequence 노드
/// 자식 노드 모두가 성공할 때까지 순차적 실행
/// </summary>
public class SequenceNode : Node
{
    public SequenceNode(string name, params Node[] childs)
    {
        this.name = name;
        children.AddRange(childs);
    }

    public override NodeState Run()
    {
        foreach (var child in children)
        {
            if (child.state == NodeState.UNDEF || child.state == NodeState.RUNNING || child.hasCondition)
            {
                state = child.Run();

                // 하나라도 실행 중이거나 실패하면 반환
                if (state == NodeState.RUNNING || state == NodeState.FAILURE)
                {
                    return state;
                }
            }
        }

        // 모두 성공 시 SUCCESS 반환
        state = NodeState.SUCCESS;
        return state;
    }
}

/// <summary>
/// Selector 노드
/// 자식 노드 중 하나라도 성공할때 까지 순차적 실행
/// </summary>
public class SelectorNode : Node
{
    public SelectorNode(string name, params Node[] childs)
    {
        this.name = name;
        children.AddRange(childs);
    }

    public override NodeState Run()
    {
        foreach (var child in children)
        {
            if (child.state == NodeState.UNDEF || child.state == NodeState.RUNNING || child.hasCondition)
            {
                state = child.Run();

                // 자식 노드 중 하나라도 RUNNING이나 SUCCESS 반환 하면, 그 상태 반환
                if (state == NodeState.RUNNING || state == NodeState.SUCCESS)
                {
                    return state;
                }
            }
        }

        // 모든 자식이 실패하면 실패 반환
        state = NodeState.FAILURE;
        return state;
    }
}


/// <summary>
/// Action 노드
/// 실제 게임 로직을 담당하는 노드
/// </summary>
public class ActionNode : Node
{
    private Func<NodeState> actionFunc;     // 실제 로직을 담당할 메서드

    public ActionNode(string name, Func<NodeState> func)
    {
        this.name = name;
        this.actionFunc = func;
    }

    public override void TagCondition()
    {
        // Action 노드는 condition을 가지고 있지 않음
        hasCondition = false;
    }

    public override void AddChild(Node node)
    {
        ("Action 노드는 자식을 가지지 못함").EditorLog();
    }

    public override NodeState Run()
    {
        // 로직 실행
        state = actionFunc.Invoke();
        return state;
    }
}

/// <summary>
/// Condition 노드
/// 특정 조건을 만족하는지 확인하는 노드
/// </summary>
public class ConditionNode : Node
{
    private Func<NodeState> condictionFunc;     // 조건을 판별할 메서드

    public ConditionNode(string name, Func<NodeState> func)
    {
        this.name = name;
        condictionFunc = func;
    }

    public override void TagCondition()
    {
        // Condition 노드는 condition을 가지고 있음
        hasCondition = true;
    }

    public override void AddChild(Node node)
    {
        ("Condition 노드는 자식을 가지지 못함").EditorLog();
    }

    public override NodeState Run()
    {
        // 조건 실행
        state = condictionFunc.Invoke();

        if (state == NodeState.RUNNING)
        {
            ("Condition 메서드는 RUNNING을 반환 못함").EditorLog();
        }

        // 조건 상태 반환
        return state;
    }
}


/// <summary>
/// AI 행동을 제어아흔 BehaviorTree 클래스
/// Root가 되는 Node를 가지고 있음
/// </summary>
public class BehaviorTree 
{
    private Node _root;

    public BehaviorTree(Node root)
    {
        this._root = root;
        _root.TagCondition();
    }

    public void RunTree()
    {
        NodeState result = _root.Run();

        if(result == NodeState.SUCCESS)
        {
            _root.Reset();
        }
    }
}
