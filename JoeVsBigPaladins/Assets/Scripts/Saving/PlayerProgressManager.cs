using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour{
    
    public PlayerProgress playerProgress = null;
    private string savePath;

    private void Awake(){
        //find save path
        savePath = Application.persistentDataPath + "/progress.json";
        //create new player progress
        this.playerProgress = new PlayerProgress();
    }

    [ContextMenu("Save")]
    public void save() {
        //saves progress
        LoaderSaver.SavePlayerProgressAsJSON(savePath, this.playerProgress);
    }

    [ContextMenu("Load")]
    public void load(){
        //loads progress
        this.playerProgress = LoaderSaver.LoadPlayerProgressFromJSON(savePath);
    }
}
