using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{

    [SerializeField] GameObject signInfo;

    private void Start(){
        //hide the sign info
        signInfo.SetActive(false);
    }

    //show sign text
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            //set canvas as active
            signInfo.SetActive(true);
        }
        
    }

    //hide sign text
    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            //set canvas as inactive
            signInfo.SetActive(false);
        }
    }


}
