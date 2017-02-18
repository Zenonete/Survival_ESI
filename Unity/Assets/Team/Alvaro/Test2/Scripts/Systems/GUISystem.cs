using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUISystem : MonoBehaviour 
{
	public Slider lifeSlider;
	public Text levelInfo;
	public Char formOfLifePlayer;

	// Use this for initialization
	void Start () 
	{
		lifeSlider.maxValue = formOfLifePlayer.lifeMax;
		lifeSlider.value = formOfLifePlayer.life;
	}
	
	void Update()
	{
		lifeSlider.value = formOfLifePlayer.life;
		levelInfo.text = "H: " + (int)formOfLifePlayer.life + " / " + formOfLifePlayer.lifeMax + " // " 
			+ "Lv: " + formOfLifePlayer.level + " // " 
			+ "Exp: " + formOfLifePlayer.experience + " / " + formOfLifePlayer.nextLevel; 
	}
}
