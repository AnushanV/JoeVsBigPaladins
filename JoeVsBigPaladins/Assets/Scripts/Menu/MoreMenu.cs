using UnityEngine;
using UnityEngine.SceneManagement;

public class MoreMenu : MonoBehaviour{

    //player progress
    [SerializeField] PlayerProgressManager progressManager;
    PlayerProgress playerProgress;

    //clears save data
    public void clearSaveData(){
        Debug.Log("Clear save data");

        //create new object
        PlayerProgress newPlayerProgress = new PlayerProgress();
        this.progressManager.playerProgress = newPlayerProgress;

        //overwrite save
        this.progressManager.save();
    }

    //returns to main menu
    public void back(){
        SceneManager.LoadScene("MainMenu");
    }
}
