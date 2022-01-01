using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHouse : MonoBehaviour
{
    [SerializeField] private Transform entrance;
    [SerializeField] private AudioSource doorSound;

    #region Unity Functions

    private void Start()
    {
        doorSound = GetComponentInChildren<AudioSource>();
    }

    #endregion

    public Vector3 GetEntrance() => entrance.position;

    public void EnterHouse(Agent agent)
    {
        StartCoroutine(AgentEntered(agent));
    }

    private IEnumerator AgentEntered(Agent agent)
    {
        doorSound.Play();
        agent.ChangeAgentVisability(false);

        Debug.Log("Character Entered House");
        yield return new WaitForSeconds(20f);

        agent.info.tiredness = 100;

        doorSound.Play();
        agent.ChangeAgentVisability(true);
    }

}
