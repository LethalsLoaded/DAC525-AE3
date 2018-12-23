/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */


using System;
using System.Linq;
using UnityEngine;

/// <summary>
///     The different states of the game.
/// </summary>
public enum GameState
{
    MENU, // Game is currently in the menu
    PAUSE, // Game is currently paused
    PLAYING, // All physics are active and calculating, game is in progress
    ANIMATION // State in which player is doing a 'final' animation for level. IE Death, or win
}

public class GameManager : MonoBehaviour
{
    #region PUBLIC_VARIABLES_HIDDEN_INSPECTOR

    [HideInInspector] public uint levelIndex = 0;

    #endregion

    #region PRIVATE_METHODS

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        lives = startingLives;
    }

    #endregion

    #region PUBLIC_VARIABLES

    [Header("- Gameplay Information -")]
    public int startingLives = 3; // The lives that player will start with
    public int maxLives = 3; // The maximum allowed lives as player can collect more throughout the levels

    [Header("- Rooms -")]
    public GameLevel[] playableLevels;

    [Header("- Object Variables -")]
    public GameObject notificationBoxUI;
    public GameObject controlsUI;
    public GameObject playerCharacter;
    public Entity playerEntity;

    #endregion

    #region PRIVATE_VARIABLES

    private static GameManager instance;
    private int lives;

    #endregion

    #region PUBLIC_METHODS

    /// <summary>
    ///     Returns the amount of current lives.
    /// </summary>
    /// <returns>Int of lives</returns>
    /// <example>
    ///     <code>
    /// int localLives = GameManager.GetLives();
    /// if (localLives > 0)
    /// 	Debug.Log($"Player is alive with {localLives} lives at the time.");
    /// else
    /// 	Debug.Log($"Player is dead.true");
    /// </code>
    /// </example>
    public int GetLives()
    {
        return lives;
    }

    /// <summary>
    ///     Checks if there is a life to remove and if so removes it. If there
    ///     is no life to remove the game will stop and an end screen will be
    ///     displayed. (No animation is played or alike, simply removes a life)
    /// </summary>
    public void RemoveLife()
    {
        lives--;
        if(lives == 0)
        {
            // TODO: do some end game shit here
        }
    }

    /// <summary>
    ///     Adds *X* lives if possible. If not, will return false
    /// </summary>
    /// <param name="livesToAdd">Amount of lives to add, default = 1</param>
    /// <returns>True if success false if failed</returns>
    public bool AddLife(int livesToAdd = 1)
    {
        // Check if we can add a life, else return false
        if (lives + 1 > maxLives) return false;
        lives += livesToAdd;
        return true;
    }

    /// <summary>
    ///     Returns an instance of the Game Manager.
    ///     If null, throws an exception.
    /// </summary>
    /// <returns>Instance of GameManager</returns>
    public static GameManager GetInstance()
    {
        if (instance == null) throw new Exception("Game Manager hasn't been initialaized.");
        return instance;
    }

    public static void PrepareLevel()
    {
        Debug.Log("Preparing level...");
		GameManager.GetInstance().controlsUI.SetActive(true);
		GameManager.GetInstance().playerCharacter.SetActive(true);

		GameManager.GetInstance().playerCharacter.transform.position = GameLevel.GetActiveLevel().levelStartPoint;
		InputManager.GetInstance()._onNotificationClose.RemoveAllListeners();
        Debug.Log("Level prepared.");
    
    }

    public void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        playerEntity = playerCharacter.GetComponent<Rogue_Script>();
    }

    #endregion
}