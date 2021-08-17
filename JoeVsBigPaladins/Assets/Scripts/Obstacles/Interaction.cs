using UnityEngine;
using UnityEngine.UI;
public class Interaction : MonoBehaviour{
    
    public bool touchingKey = false;
    public GameObject currentKey = null;
    public GameObject keyCount;
    public Text interactionText;

    [SerializeField] AudioSource shimmeringSound;
    void Update(){

        //check if player is near key and inputs pick up
        if (touchingKey && Input.GetKeyDown(KeyCode.E) && currentKey != null) {

            if (currentKey) { //if current key exists
                shimmeringSound.Play(); //play sound effect

                //create particle effect
                GameObject particles = GameObject.Instantiate(Resources.Load("KeyEffect"), transform.position, Quaternion.identity) as GameObject;
                Destroy(particles, 0.3f);

                //destroy the key and increment number of keys
                Destroy(currentKey);
                keyCount.GetComponent<UpdateKeys>().addKey();
                interactionText.text = "";
                currentKey = null;
                touchingKey = false;
            }
        }
    }
}
