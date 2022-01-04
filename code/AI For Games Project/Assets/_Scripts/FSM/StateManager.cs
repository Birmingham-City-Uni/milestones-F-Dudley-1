using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    /// <summary>
    /// The State Which Contains The Current Active States.
    /// </summary>
    private Stack<State> stack;

    #region Main Functions
    /// <summary>
    /// The Starting Function of The StateManager.
    /// </summary>
    /// <param name="startingState">The First State To Go On The Stack.</param>
    public void Init(State startingState)
    {
        this.stack = new Stack<State>();
        stack.Push(startingState);

        getCurrentState().Enter();
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    /// <returns></returns>
    public bool Update()
    {
        if (hasState()) return stack.Peek().Execute();
        else return false;
    }
    #endregion

    #region Stack Functions
    /// <summary>
    /// Gets The Current Top State On The Stack.
    /// </summary>
    /// <returns>Returns The Current Active State.</returns>
    public State getCurrentState() => hasState() ? stack.Peek() : null;

    /// <summary>
    /// Checks if The StateManager has a State. 
    /// </summary>
    /// <returns>True if the StateManager has a State, False if the StateManager is Empty.</returns>
    public bool hasState() => stack.Count > 0;

    /// <summary>
    /// Removes The Top of The Stack.
    /// </summary>
    /// <returns>True if a State is Removed, False if Nothing is Removed.</returns>
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

    /// <summary>
    /// Adds A State To The StateManager.
    /// </summary>
    /// <param name="newState">The New State To Add To The StateManager.</param>
    /// <returns>True if The State is Added, False if it Could Not Be Added.</returns>
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
