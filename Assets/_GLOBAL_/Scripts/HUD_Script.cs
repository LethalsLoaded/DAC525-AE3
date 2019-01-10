using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Script : MonoBehaviour {

	public Sprite heart;
	public Sprite emptyHeart;
	public Image[] live;

	int lives;

	void Update ()
	{
		 lives = GameManager.GetInstance().GetLives();

		switch (lives)
		{
			case 3:
 				live[0].sprite = heart;
				live[1].sprite = heart;
				live[2].sprite = heart;
					break;
			case 2:
				live[0].sprite = heart;
				live[1].sprite = heart;
				live[2].sprite = emptyHeart;
					break;
			case 1:
				live[0].sprite = heart;
				live[1].sprite = emptyHeart;
				live[2].sprite = emptyHeart;
					break;
			case 0:
				live[0].sprite = emptyHeart;
				live[1].sprite = emptyHeart;
				live[2].sprite = emptyHeart;
					break;
			default:
			break;
		}
	}
}
