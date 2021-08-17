using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour{
    
    //used to load from save
    [SerializeField] PlayerProgressManager progressManager;
    PlayerProgress playerProgress;
    
    //starts the game
    public void play() {

        //path to save file
        string savePath = Application.persistentDataPath + "/progress.json";
            
        //create new save file if it doesnt exist
        if (!File.Exists(savePath)){
            PlayerProgress newSave = new PlayerProgress();
            string json = JsonUtility.ToJson(newSave);
            File.WriteAllText(savePath, json);
        }
        
        //loads save
        progressManager.load();
        playerProgress = progressManager.GetComponent<PlayerProgressManager>().playerProgress;

        //get the level number from save
        int levelNum = playerProgress.levelNum;
        
        //load from save
        SceneManager.LoadScene(levelNum);
    }

    //switch to more menu
    public void more(){
        SceneManager.LoadScene("MoreMenu");
    }

    //exit game
    public void exit(){
        Debug.Log("EXIT");
        Application.Quit();
    }
}
