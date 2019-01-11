using UnityEngine;

public class MenuManager : MonoBehaviour {

	

	public void StartGame()
	{
		var nextLevel = GameManager.GetInstance().playableLevels[GameManager.GetInstance().levelIndex];
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel.GetSceneName());
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}
