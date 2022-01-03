using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHouse : MonoBehaviour
{
    public Transform entrance;
    [SerializeField] private AudioSource doorSound;

    #region Unity Functions

    private void Start()
    {
        doorSound = GetComponentInChildren<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(entrance.position, 1.75f);
    }

    #endregion

    public Vector3 GetEntrance() => entrance.position;

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

    public void EnterHouseTired(Agent agent)
    {
        StartCoroutine(AgentEnteredTired(agent));
    }

    public void EnterHouseAlerted(Agent agent)
    {
        StartCoroutine(AgentEnteredAlerted(agent));
    }

    private IEnumerator AgentEnteredHungry(Agent agent)
    {
        doorSound.Play();
        agent.ChangeAgentVisability(false);

        Debug.Log("CharacterEntered House - Hungry");
        yield return new WaitForSeconds(10f);

        agent.info.Hunger += 50;

        doorSound.Play();
        agent.ChangeAgentVisability(true);
        MoveAgentToEntrance(agent);
    }

    private IEnumerator AgentEnteredTired(Agent agent)
    {
        doorSound.Play();
        agent.ChangeAgentVisability(false);

        Debug.Log("Character Entered House - Tired");
        yield return new WaitForSeconds(20f);

        agent.info.Tiredness = 100;

        doorSound.Play();
        agent.ChangeAgentVisability(true);
        MoveAgentToEntrance(agent);
    }

    private IEnumerator AgentEnteredAlerted(Agent agent)
    {
        doorSound.Play();
        agent.ChangeAgentVisability(false);

        Debug.Log("CharacterEntered House - Hungry");
        yield return new WaitForSeconds(15f);

        agent.info.isAlerted = false;

        doorSound.Play();
        agent.ChangeAgentVisability(true);
        MoveAgentToEntrance(agent);
    }

    private void MoveAgentToEntrance(Agent agent)
    {
        agent.transform.SetPositionAndRotation(entrance.position, entrance.rotation);
    }
}
