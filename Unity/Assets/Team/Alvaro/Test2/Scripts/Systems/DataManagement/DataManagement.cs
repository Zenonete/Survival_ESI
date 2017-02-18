using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManagement : MonoBehaviour 
{
	public static DataManagement dataManagement;
	Char player;

	void Awake () 
	{
		if(dataManagement == null)
		{
			DontDestroyOnLoad(gameObject);
			dataManagement = this;
		}
		else if(dataManagement != this)
		{
			Destroy(gameObject);
		}

		player = GameObject.Find("ThirdPersonController").GetComponent<Char>();

		Load();
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/SaveData01.dat");

		PlayerData data = new PlayerData();		//De momento esto por test**************************************

		data.maxHealth = player.lifeMax;
		data.experience = player.experience;

		bf.Serialize(file, data);
		file.Close();

	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/SaveData01.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/SaveData01.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			player.lifeMax = data.maxHealth;
			player.experience = data.experience;
			player.SetAttributes(false);
		}
	}
}

//Clase para test de salvado de datos*****************************************************************************
[Serializable]
class PlayerData
{
	public float maxHealth = 100;
	public int experience = 0;
}
