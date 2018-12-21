using UnityEngine;

public class MenuManager : MonoBehaviour {

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	public void StartGame()
	{
		var nextLevel = GameManager.GetInstance().playableLevels[GameManager.GetInstance().levelIndex];
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel.GetSceneName());
	}
}
