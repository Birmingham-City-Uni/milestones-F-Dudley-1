using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [Header("Menu References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject subMenu;

    #region Button Functions
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetPathfindingType(int selectedAlgorithm)
    {
        PathFinding.currentChosenAlgorithm = (PathfindingAlgorithms) selectedAlgorithm;
    }

    #endregion
}
