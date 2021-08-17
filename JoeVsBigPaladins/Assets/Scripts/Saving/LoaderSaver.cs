using UnityEngine;
using System.IO;

public class LoaderSaver : MonoBehaviour{

    //saves progress into a json file
    public static void SavePlayerProgressAsJSON(string savePath, PlayerProgress progress) {        
        
        //convert save to json and write to file
        string json = JsonUtility.ToJson(progress);
        File.WriteAllText(savePath, json);
    }

    //loads save from json file
    public static PlayerProgress LoadPlayerProgressFromJSON(string savePath) {

        //check if the save file exists
        if (File.Exists(savePath)){

            //load the save from json
            string json = File.ReadAllText(savePath);
            PlayerProgress progress = JsonUtility.FromJson<PlayerProgress>(json);

            //return the progress object
            return progress;
        }
        else {
            Debug.LogError("Unable to load file: " + savePath);
        }

        return null;
    }
}
