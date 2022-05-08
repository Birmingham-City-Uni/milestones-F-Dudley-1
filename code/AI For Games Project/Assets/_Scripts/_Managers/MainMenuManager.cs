using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [Header("Menu References")]

    /// <summary>
    /// The GameObject For The MainMenu.
    /// </summary>
    [SerializeField] private GameObject mainMenu;

    /// <summary>
    /// The GameObjet For The SubMenu.
    /// </summary>
    [SerializeField] private GameObject subMenu;

    #region Button Functions

    /// <summary>
    /// Starts The Game Scene From The MenuScene.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Quits The Game Application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Changes The Pathfinding Algorithm to the Inputted Parmeter.
    /// </summary>
    /// <param name="selectedAlgorithm">The Index of The Desired Pathfinding Algorithm.</param>
    public void SetPathfindingType(int selectedAlgorithm)
    {
        PathFinding.currentChosenAlgorithm = (PathfindingAlgorithms) selectedAlgorithm;
    }

    #endregion
}
