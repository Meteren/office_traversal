using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    public Dictionary<Type,StateNode> stateNodes = new Dictionary<Type,StateNode>();
    public IState currentState;
    public void Update()
    {
        Transition transition = GetTransition();

        if (transition != null)
            ChangeState(transition.To);

        currentState.Update();

    }

    public Transition GetTransition()
    {
        foreach(var transition in stateNodes[currentState.GetType()].transitions)
        {
            if (transition.Execute())
            {
                return transition;
            }
        }
        return null;    
    }

    public void ChangeState(IState to)
    {
        if(currentState != null)
            currentState.OnExit();

        currentState = to;

        currentState.OnStart();
    }


    public void AddTransition(IState from, IState to, IPredicate predicate) =>
        AddOrGetStateNode(from).AddTransition(new Transition(AddOrGetStateNode(to).State,predicate));


    private StateNode AddOrGetStateNode(IState state)
    {
        StateNode stateNode;
        if (stateNodes.TryGetValue(state.GetType(), out StateNode sNode))
            stateNode = sNode;
        else
        {
            stateNode = new StateNode(state);
            stateNodes[state.GetType()] = stateNode;
        }

        return stateNode;   
    }

    public class StateNode
    {
        IState state;
        public IState State { get { return state; } }
        public List<Transition> transitions = new List<Transition>();

        public StateNode(IState state) { this.state = state; }
        public void AddTransition(Transition transition) => transitions.Add(transition);

    }

}





