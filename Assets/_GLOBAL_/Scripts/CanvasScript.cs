﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	static CanvasScript _instance;

	void Start ()
	{
		if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}

	public void HideNotificationBoxNonStatic()
	{
		GameManager.GetInstance().notificationBoxUI.SetActive(false);
	}

	public static void HideNotificationBox()
	{
		GameManager.GetInstance().notificationBoxUI.SetActive(false);
	}

	public static void ShowNotificationBox()
	{
		GameManager.GetInstance().notificationBoxUI.SetActive(true);
	}

	public static void SetNotificationBoxTitle(string newText)
	{
		var headerTree = GameManager.GetInstance().notificationBoxUI.GetComponentsInChildren<Transform>().First(x=>x.name == "Header").gameObject;
		var textBox = headerTree.transform.GetChild(0);

		textBox.GetComponent<Text>().text = newText;
	}

	public static void SetNotificationBoxText(string newText)
	{
		var textTree = GameManager.GetInstance().notificationBoxUI.GetComponentsInChildren<Transform>().First(x=>x.name == "Text_Padding").gameObject;
		var textBox = textTree.transform.GetChild(0).GetChild(0).GetChild(0);

		textBox.GetComponent<Text>().text = newText;
	}

	public static void SetNotificationBoxCloseText(string newText)
	{
		var headerTree = GameManager.GetInstance().notificationBoxUI.GetComponentsInChildren<Transform>().First(x=>x.name == "Close_Button").gameObject;
		var textBox = headerTree.transform.GetChild(0);

		textBox.GetComponent<Text>().text = newText;
	}
}