using UnityEngine;
using UnityEngine.UI;

public class KeyInteraction : MonoBehaviour{
    public GameObject keyCount;
    public Text interactionText;

    GameObject player;

    private void Start(){

        //get components
        player = GameObject.FindGameObjectWithTag("Joe");
        keyCount = GameObject.FindGameObjectWithTag("KeyCount");
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<Text>();
    }

    //displays a message to pick up key
    private void OnTriggerEnter(Collider other){

        //check if colliding with player
        if (other.gameObject.CompareTag("Player")) {

            //display message and tell player that they are touching a key
            interactionText.text = "Press \"E\" to Collect Key";
            player.GetComponent<Interaction>().currentKey = gameObject;
            player.GetComponent<Interaction>().touchingKey = true;
        }
    }

    //removes message
    private void OnTriggerExit(Collider other){
        
        //check if colliding with player
        if (other.gameObject.CompareTag("Player")) {

            //remove message and references to key
            interactionText.text = "";
            player.GetComponent<Interaction>().currentKey = null;
            player.GetComponent<Interaction>().touchingKey = false;
        }
    }


}
