using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Dialogue_Script : MonoBehaviour {

	public Text displayText;
	public GameObject dialogue;
	public GameObject speechIcon;
	public string[] sentences;
	int index;
	public float speechSpeed;
	public GameObject continueButton, skipButton;


	// Use this for initialization
	void Start () 
	{
		StartCoroutine (Text());
	}

	void Update ()
	{
		if (displayText.text == sentences[index])
		{
			skipButton.SetActive(false);
			continueButton.SetActive(true);
		}
		else
		{
			skipButton.SetActive(true);
			continueButton.SetActive(false);
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "PLAYER")
		{
			speechIcon.SetActive(true);
			StartCoroutine("TextBox");
		}
	}
	void OnTriggerExit2D (Collider2D col)
	{
		if (col.tag == "PLAYER")
		{
			speechIcon.SetActive(false);
			StopCoroutine("TextBox");
		}
	}
	IEnumerator TextBox()
	{
		yield return new WaitForSeconds(2);
		dialogue.SetActive(true);
	}

	IEnumerator Text ()
	{
		foreach (char characters in sentences[index].ToCharArray())
		{
			displayText.text += characters;
			yield return new WaitForSeconds(speechSpeed);
		}
	}
	public void ContinueButton()
	{
		displayText.text = null;
		//speechSpeed = 0.1f;
		if (index < sentences.Length -1)
		{
			index++;
			continueButton.SetActive(false);
			StartCoroutine (Text());
		}
		else
		{
			dialogue.SetActive(false);
		}
	}
	public void SkipButton ()
	{
		//speechSpeed = 0;
		StopAllCoroutines();
		displayText.text = null;
		displayText.text = sentences[index];
		skipButton.SetActive(false);
		continueButton.SetActive(true);
	}
}
