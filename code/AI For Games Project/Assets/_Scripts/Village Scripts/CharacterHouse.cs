using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHouse : MonoBehaviour
{
    /// <summary>
    /// The Entrance Location To The Current House.
    /// </summary>
    public Transform entrance;

    /// <summary>
    /// The Audio of The Door Entrance Opening.
    /// </summary>
    [SerializeField] private AudioSource doorSound;

    #region Unity Functions

    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    private void Start()
    {
        doorSound = GetComponentInChildren<AudioSource>();
    }

    /// <summary>
    /// Unity OnDrawGizmos Function.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(entrance.position, 1.75f);
    }

    #endregion

    /// <summary>
    /// Gets The Entrance Location of The House.
    /// </summary>
    /// <returns>The Position of The Houses Entrance.</returns>
    public Vector3 GetEntrance() => entrance.position;

    /// <summary>
    /// Moves The Agent Into a State of Inside Their House.
    /// </summary>
    /// <param name="agent">The Agent To Enter The House.</param>
    public void EnterHouse(Agent agent)
    {
        if (agent.info.isAlerted)
        {
            StartCoroutine(AgentEnteredAlerted(agent));
        }
        else if (agent.info.Hunger <= agent.info.hungryThreshold)
        {
            StartCoroutine(AgentEnteredHungry(agent));
        }
        else if (agent.info.Tiredness <= agent.info.tirednessThreshold)
        {
            StartCoroutine(AgentEnteredTired(agent));
        }
    }

    /// <summary>
    /// Enters An Agent Into The House In a Tired State.
    /// </summary>
    /// <param name="agent">The Agent To Enter The House.</param>
    public void EnterHouseTired(Agent agent)
    {
        StartCoroutine(AgentEnteredTired(agent));
    }

    /// <summary>
    /// Enters An Agent Into The House In a Alerted State.
    /// </summary>
    /// <param name="agent">The Agent To Enter The House.</param>
    public void EnterHouseAlerted(Agent agent)
    {
        StartCoroutine(AgentEnteredAlerted(agent));
    }

    /// <summary>
    /// A Coroutine That Runs If The Agent Enters The House In a Hungry State.
    /// </summary>
    /// <param name="agent">The Agent To Enter The House.</param>
    /// <returns>A Coroutine.</returns>
    private IEnumerator AgentEnteredHungry(Agent agent)
    {
        doorSound.Play();
        agent.ChangeAgentVisability(false);

        Debug.Log("CharacterEntered House - Hungry");
        yield return new WaitForSeconds(10f);

        agent.info.Hunger += 50;
        agent.info.HasJobLocation = false;

        doorSound.Play();
        agent.ChangeAgentVisability(true);
        MoveAgentToEntrance(agent);
    }

    /// <summary>
    /// A Coroutine That Runs If The Agent Enters The House In a Tired State.
    /// </summary>
    /// <param name="agent">The Agent To Enter The House.</param>
    /// <returns>A Coroutine.</returns>
    private IEnumerator AgentEnteredTired(Agent agent)
    {
        doorSound.Play();
        agent.ChangeAgentVisability(false);

        Debug.Log("Character Entered House - Tired");
        yield return new WaitForSeconds(20f);

        agent.info.Tiredness = 100;
        agent.info.HasJobLocation = false;

        doorSound.Play();
        agent.ChangeAgentVisability(true);
        MoveAgentToEntrance(agent);
    }

    /// <summary>
    /// A Coroutine That Runs If The Agent Enters The House In a Alerted State.
    /// </summary>
    /// <param name="agent">The Agent To Enter The House.</param>
    /// <returns>A Coroutine.</returns>
    private IEnumerator AgentEnteredAlerted(Agent agent)
    {
        doorSound.Play();
        agent.ChangeAgentVisability(false);

        Debug.Log("CharacterEntered House - Hungry");
        yield return new WaitForSeconds(15f);

        agent.info.isAlerted = false;
        agent.info.HasJobLocation = false;

        doorSound.Play();
        agent.ChangeAgentVisability(true);
        MoveAgentToEntrance(agent);
    }

    /// <summary>
    /// Moves The Selected Agent To The Entrance Position of The House.
    /// </summary>
    /// <param name="agent"></param>
    private void MoveAgentToEntrance(Agent agent)
    {
        agent.transform.SetPositionAndRotation(entrance.position, entrance.rotation);
    }
}
