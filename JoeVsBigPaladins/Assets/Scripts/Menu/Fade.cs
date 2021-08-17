using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour{

    public Animator animator;
    public float fadeDuration;
    
    //loads a scene
    public void LoadNextScene(int sceneIndex) {
        StartCoroutine(LoadLevel(sceneIndex)); //load next level
    }

    IEnumerator LoadLevel(int sceneIndex) {
        //display fade
        animator.SetTrigger("startFade");

        //delay then load scene
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneIndex);
    }
         
}
