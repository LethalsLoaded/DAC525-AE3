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
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameLevel
{
    public string levelName = "Generic Name";
    public uint levelNumber = 0;
    public Vector2 levelStartPoint;


    private string sceneName;
    public SceneReference sceneReference;

    /// <summary>
    ///     Populates the
    ///     <param name="sceneName">sceneName</param>
    ///     variable with
    ///     the scene name from the
    ///     <param name="sceneReference" SceneReference path.
    /// </summary>
    private void CreateSceneName()
    {
        var path = sceneReference.ScenePath;
        sceneName = Path.GetFileNameWithoutExtension(path);
    }

    /// <summary>
    ///     Returns the name of the scene linked to the GameLevel.
    ///     Creates a
    ///     <param name="sceneName">Scene Name</param>
    ///     if it's not set.
    /// </summary>
    /// <returns>Name of the scene</returns>
    public string GetSceneName()
    {
        if (string.IsNullOrEmpty(sceneName)) CreateSceneName();
        return sceneName;
    }

    /// <summary>
    ///     Gets the active Game Level
    /// </summary>
    /// <returns>Instance of active GameLevel</returns>
    public static GameLevel GetActiveLevel()
    {
        var scene = SceneManager.GetActiveScene();

        return GameManager.GetInstance().playableLevels.First
        (
            x => x.GetSceneName().ToLower() == scene.name.ToLower()
        );
    }

    public static void NextLevel()
    {
        GameManager.GetInstance().levelIndex++;
        var nextLevel = GameManager.GetInstance().playableLevels[GameManager.GetInstance().levelIndex];
        SceneManager.LoadScene(nextLevel.GetSceneName());
        var spawnPoint = GameObject.Find("PLAYER_SPAWN_POINT").transform.position;
        GameObject.FindGameObjectWithTag("PLAYER").transform.position = spawnPoint;
    }
}