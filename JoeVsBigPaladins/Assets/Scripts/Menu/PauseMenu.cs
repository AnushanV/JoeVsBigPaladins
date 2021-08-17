using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour{

    public static bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject progressManager;

    private void Start(){
        //allow mouse to move
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        //toggle menu with esc
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused){
                resume();
            }
            else {
                pause();
            }
        }
    }

    //open pause menu
    void pause(){
        //enable cursor and open menu
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true); 
        Time.timeScale = 0f; 
        isPaused = true;
    }

    //resume game
    public void resume() {
        //disable cursor and close menu
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    
    //save game
    public void save() {
        //create a PlayerProgress object with current scene index
        PlayerProgress currentProgress = new PlayerProgress(SceneManager.GetActiveScene().buildIndex);
        
        //save data
        progressManager.GetComponent<PlayerProgressManager>().playerProgress = currentProgress;
        progressManager.GetComponent<PlayerProgressManager>().save();
        
    }

    //save and exit
    public void saveAndExit() {
        save(); //save
        
        //return to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
