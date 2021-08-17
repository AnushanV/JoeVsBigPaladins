using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour{

    //collision
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundPosition;
    private float groundDistance = 0.1f;

    //navigation
    [SerializeField] private bool repeat = true;
    private NavMeshAgent agent = null;
    private Animator animator = null;
    private int currentWaypointIndex = 0;
    private bool patrolling = true;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float closeEnoughDistance = 1f;
    public bool isAggro = true;

    //speed
    public float maxSpeed = 1f;
    public float speed = 1f;
    
    //player
    public GameObject player;

    //attack pattern
    public float attackFrequency = 3f;
    float attackTimer = 3f;

    //states
    public bool isGrounded = true;
    public bool isKneeling = false;
    private bool followPlayer = false;

    void Awake(){
        //get required components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start(){
        //start navigating to the first waypoint
        if (agent != null && (waypoints.Length > 0)) {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        animator.SetBool("moving", true);
    }

    void Update(){

        //dont do anything if not patrolling
        if (!patrolling){
            return;
        }
        //follow player if aggro set
        else if (isAggro){

            //find relative height to player
            float heightDiff = 0f;
            if (player) {
                heightDiff = player.transform.position.y - transform.position.y;
            }

            attackTimer -= Time.deltaTime; //decrease attack timer

            //choose attack based on height difference
            if (attackTimer <= 0){
                attackTimer = attackFrequency;
                //kick if height difference is low
                if (heightDiff < 5){
                    animator.SetTrigger("kick");
                }
                //slash if height difference is high
                else{
                    animator.SetTrigger("attack");
                }
            }

            //turn towards player while aggro'd
            if (player) {
                transform.LookAt(player.transform);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
        //walk towards the player
        else if (followPlayer && player) {
            //set navigation to player
            agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
        //move between waypoints
        else if (!isKneeling){
            
            //travel to next waypoint
            float distanceToTarget = Vector3.Distance(agent.transform.position, waypoints[currentWaypointIndex].position);
            if (distanceToTarget < closeEnoughDistance){
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length){
                    //loop waypoints
                    if (repeat){ 
                        currentWaypointIndex = 0;
                    }
                    //end navigation
                    else{
                        patrolling = false;
                        animator.SetBool("moving", false);
                        return;
                    }
                }
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }

        //move to ground
        if (isKneeling) {
            isGrounded = Physics.CheckSphere(groundPosition.position, groundDistance, groundLayer);
            if (!isGrounded){
                //apply gravity
                transform.position = new Vector3(transform.position.x, transform.position.y - 9.81f * Time.deltaTime, transform.position.z);
            }

            //modify states
            capsuleCollider.height = 1.0f;
            agent.enabled = false;
            isAggro = false;
            followPlayer = false;
        }
    }

    //sets the enemy to aggro state
    public void enterAggro() {
        //stop movement
        agent.speed = 0f; 
        animator.SetBool("moving", false);
        
        isAggro = true;
        
        //start following player
        followPlayer = true;

    }

    //removes aggro from enemy
    public void exitAggro() {
        //reset speed
        agent.speed = maxSpeed;
        animator.SetBool("moving", true);
        isAggro = false;
    }
}
