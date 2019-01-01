using UnityEngine;

public class Level_17_Level_Manager : MonoBehaviour {

    private bool _shake = false;

	public float shakeDuration = 0.0f;
    public float shakeAmount = 0.3f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Start()
    {
        originalPos = Camera.main.transform.position;
    }
	
	void Update ()
	{
        if(!_shake) return;

        if (shakeDuration > 0)
		{
			Camera.main.transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
			
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			Camera.main.transform.position = originalPos;
            _shake = false;
}
	}

    public void Shake()
    {
        originalPos = GameManager.GetInstance().playerCharacter.transform.position;
        _shake = true;
        shakeDuration = 1.0f;
    }
}

// Camera shake script inspired by https://gist.github.com/ftvs/5822103
