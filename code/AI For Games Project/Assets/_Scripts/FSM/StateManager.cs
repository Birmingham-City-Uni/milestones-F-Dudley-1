using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private Stack<State> stack;

    #region Main Functions
    public void Init(State startingState)
    {
        this.stack = new Stack<State>();
        stack.Push(startingState);

        getCurrentState().Enter();
    }

    public bool Update()
    {
        if (hasState()) return stack.Peek().Execute();
        else return false;
    }
    #endregion

    #region Stack Functions
    public State getCurrentState() => hasState() ? stack.Peek() : null;
    public bool hasState() => stack.Count > 0;

    public bool popState()
    {
        if (stack.Count > 0)
        {
            getCurrentState().Exit();
            stack.Pop();

            return true;
        }
        else return false;
    }

    public bool pushState(State newState)// Could Add '&& !Stack.Contains(newState)' to avoid repetition of states.
    {
        if (stack.Peek() != newState && !stack.Contains(newState))
        {
            stack.Push(newState);
            getCurrentState().Enter();

            return true;
        }
        else return false;
    }
    #endregion
}
