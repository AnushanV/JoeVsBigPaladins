using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAggro : MonoBehaviour{

    //enemy
    public GameObject enemy;

    //set aggro on entry
    private void OnTriggerEnter(Collider other){

        //check if colliding with player
        if (other.gameObject.CompareTag("Player")){
            Debug.Log("Player Entered Aggro Zone");
            //set aggro
            enemy.GetComponent<EnemyMovement>().enterAggro();
        }
    }

    //remove aggro on exit
    private void OnTriggerExit(Collider other){
        //check if colliding with player
        if (other.gameObject.CompareTag("Player")){
            Debug.Log("Player Exited Aggro Zone");
            //remove aggro
            enemy.GetComponent<EnemyMovement>().exitAggro();
        }
    }
}
