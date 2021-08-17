using UnityEngine;

public class MovingPlatform : MonoBehaviour{

    //points to move between
    public Transform startPoint;
    public Transform endPoint;
    Vector3 currentPos;
    Vector3 nextPoint;

    public float moveSpeed = 5f;

    public GameObject player;

    void Start(){
        //get positions
        currentPos = startPoint.position;
        transform.position = currentPos;
        nextPoint = endPoint.position;
    }

    void Update(){
        //move towards to the point
        transform.position = Vector3.MoveTowards(transform.position, nextPoint, moveSpeed*Time.deltaTime);

        //change points
        if (transform.position == nextPoint) {
            if (currentPos == startPoint.position){
                currentPos = endPoint.position;
                nextPoint = startPoint.position;
            }
            else {
                currentPos = startPoint.position;
                nextPoint = endPoint.position;
            }
        }
    }

    //set player as child object
    private void OnTriggerEnter(Collider other){
        //Debug.Log("JOE: " + other.tag);
        if (other.CompareTag("Player")){
            player.transform.parent = transform;
        }
    }

    //remove player from child
    private void OnTriggerExit(Collider other){
        //Debug.Log("jOE");
        if (other.CompareTag("Player")){
            player.transform.parent = null;
        }
    }
}
