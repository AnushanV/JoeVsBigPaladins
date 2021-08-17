using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateLevel : MonoBehaviour{
    
    public GameObject keys;
    public GameObject levelLoader;
    public Text interactionText;

    [SerializeField] AudioSource shimmerSound;
    //advances player to next level
    private void OnTriggerEnter(Collider other){

        //check if colliding with player
        if (other.gameObject.CompareTag("Player")) {
            shimmerSound.Play(); //play sound effect
            Debug.Log("Player entered level loading zone");

            //get the key values
            int numKeys = keys.GetComponent<UpdateKeys>().numKeys;
            int maxKeys = keys.GetComponent<UpdateKeys>().maxKeys;

            //load next level if there are enough keys
            if (numKeys >= maxKeys){
                interactionText.text = "Loading Next Level"; //edit interaction text

                //play animation on player
                GameObject playerObject = other.gameObject.transform.GetChild(0).gameObject;
                if (playerObject.CompareTag("Joe"))
                {
                    Animator playerAnimator = playerObject.GetComponent<Animator>();
                    playerAnimator.SetTrigger("dance");
                }
                else {
                    Destroy(other.gameObject);
                }

                StartCoroutine(LoadLevelAfterTime(3f));
                
            }
            //display message if not enough keys
            else {
                interactionText.text = "Not Enough Keys to Advance";
            }
        }
    }

    //removes interaction message
    private void OnTriggerExit(Collider other){
        int numKeys = keys.GetComponent<UpdateKeys>().numKeys;
        int maxKeys = keys.GetComponent<UpdateKeys>().maxKeys;

        if (other.gameObject.CompareTag("Player")) {
            if (numKeys < maxKeys) {
                interactionText.text = null;
            }
        }
    }

    IEnumerator LoadLevelAfterTime(float time){
        yield return new WaitForSeconds(time);
        levelLoader.GetComponent<Fade>().LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1); //load the next level
    }
}
