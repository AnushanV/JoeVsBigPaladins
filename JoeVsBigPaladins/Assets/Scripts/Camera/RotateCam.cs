using UnityEngine;

public class RotateCam : MonoBehaviour
{

    public Transform player;

    // Update is called once per frame
    void Update(){
        //copy player's rotation
        transform.rotation = player.rotation;
    }
}
