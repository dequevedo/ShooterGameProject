using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad {

    //public static List<Game> savedGames = new List<Game> ();
    //
    //	public static void Save()
    //	{
    //		SaveAndLoad.savedGames.Add (Game.current);
    //		BinaryFormatter bF = new BinaryFormatter ();
    //		FileStream file = File.Create (Application.persistentDataPath + "/savetest.sv");
    //		bF.Serialize (file, SaveAndLoad.savedGames);
    //		file.Close ();
    //	}
    //
    //	public static void Load()
    //	{
    //		if (File.Exists (Application.persistentDataPath + "/savetest.sv")) {
    //			BinaryFormatter bF = new BinaryFormatter ();
    //			FileStream file = File.Open (Application.persistentDataPath + "/savetest.sv");
    //			SaveAndLoad.savedGames = (List<Game>)bF.Deserialize (file);
    //			file.Close ();
    //		}
    //	}
    //
    private static string filePath = Application.persistentDataPath + "/savetest.sv";

    public static void SavePlayer (PlayerController player)
	{
		BinaryFormatter bF = new BinaryFormatter ();
		FileStream file = new FileStream(Application.persistentDataPath + "/savetest.sv", FileMode.Create);
	
		PlayerData data = new PlayerData (player);
		bF.Serialize (file, data);
		file.Close ();
	}

	public static float[] LoadPlayer()
	{
		if (File.Exists (Application.persistentDataPath + "/savetest.sv")) {
			BinaryFormatter bF = new BinaryFormatter ();
			FileStream file = new FileStream (Application.persistentDataPath + "/savetest.sv", FileMode.Open);
			Debug.Log (file.Name);
			PlayerData data = bF.Deserialize (file) as PlayerData;
			file.Close ();
			return data.stats;
		} else {
			return new float[1];
		}
	} 

    public static void DeleteSavedPlayer ()
    {
        if (File.Exists(Application.persistentDataPath + "/savetest.sv"))
        {
            File.Delete(filePath);
            Debug.Log("File deleted. Path: " + filePath);
        } else
        {
            Debug.LogWarning("There are no files to delete!");
        }
    }
}

[System.Serializable]
public class PlayerData {
	public float[] stats;
	public PlayerData (PlayerController player)
	{
		stats = new float[5];
		stats [0] = player.CurrentHealth;
		stats [1] = player.MaximumHealth;
		//stats [2] = (int) player.playerLives;
	}
}
