using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour{

    [SerializeField] PlayerProgressManager progressManager;

    //return to the main menu
    public void menu(){
        //load menu scene
        SceneManager.LoadScene("MainMenu");
    }

    //exit game
    public void exit(){
        Debug.Log("Exit");
        Application.Quit(); //close game
    }

    private void Start(){
        //allow cursor to move
        Cursor.lockState = CursorLockMode.None;
        //reset progress after completion
        progressManager.GetComponent<PlayerProgressManager>().save();
    }
}
