using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public Transform camPos;
    
    //camera
    public Camera cam;
    public GameObject mainCam;
    public GameObject aimCam;
    public Transform followTarget;

    //movement
    public float moveSpeed = 6f;
    public float speed = 6f;

    //turning
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //gravity and collisions
    public float gravityFactor = 1f;
    public float maxGravityFactor = 5f;
    public Transform groundPosition;
    public float groundDistance = 0.1f;
    public LayerMask groundLayer;
    public bool isGrounded = false;

    //aerial
    public float jumpSpeed = 10f;
    public float flySpeed = 0.2f;
    public float flySpeedMax = 4f;
    public bool canAirControl = true;
    public float verticalSpeed = 0f;
    bool falling = true;
    public bool isGliding = false;
    public float maxVerticalSpeed = 120f;
    public float currentVerticalSpeed = 0f;

    //dash
    bool dashing = false;
    public float dashSpeed = 12f;
    public float maxDashDuration = 1f;
    public float dashDuration = 1f;

    //wire related
    public Transform lineStartPos;
    public LineRenderer lineRenderer;
    public float lineWidth = 0.05f;
    public float lineMaxLength = 50f;
    public float lineSpeed = 30f;
    private float currentLineSpeed = 0f;
    public float rotationSpeed = 20f;
    public float maxLineLength = 50f;
    public GameObject marker;
    Vector3 moveDir = Vector3.zero;
    Vector3 lineTargetDir = Vector3.zero;
    Vector3 lineTargetMove = Vector3.zero;
    bool lineActive = false;
    bool canLine = false;
    public float maxLineDuration = 5f;
    public float lineDuration = 5f;

    //movement vectors
    Vector3 x = Vector3.zero;
    public Vector3 y = Vector3.zero;
    Vector3 z = Vector3.zero;

    //attacks
    public bool isAttacking = false;
    public float hurricaneKickDamage = 20f;
    float attackDuration = 1f;
    float attackTimer = 0f;
    [SerializeField] float hurricaneKickRadius = 1f;
    [SerializeField] float spinSpeed = 15f;
    [SerializeField] LayerMask enemyLayer;

    //stamina
    public PlayerStamina playerStamina;

    //knockback
    public Vector3 knockback = Vector3.zero;
    public bool inKnockback = false;
    public float maxKnockbackDuration = 0.8f;
    public float knockbackDuration = 0.8f;

    //sounds
    [SerializeField] AudioSource attackSound;
    [SerializeField] AudioSource effortSound;
    [SerializeField] AudioSource wireSound;

    void Start(){
        //controller = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        
        //create a line to visualize the wire
        Vector3[] initLinePositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        lineRenderer.SetPositions(initLinePositions);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        marker.GetComponent<Renderer>().enabled = false;

        //set camera as third person
        aimCam.SetActive(false);
        mainCam.SetActive(true);
    }
    
    /*
    public float GetAxisCustom(string axisName) {
        if (axisName == "Mouse Y"){
            if (!isGrounded)
            {
                return 0.5f + UnityEngine.Input.GetAxis(axisName);
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }
    */
    
    void Update(){

        //Debug.Log(marker.transform.position);

        //check for ground
        isGrounded = Physics.CheckSphere(groundPosition.position, groundDistance, groundLayer);
        if (isGrounded){
            animator.SetBool("isGrounded", true);
            gravityFactor = maxGravityFactor;
        }
        else{
            animator.SetBool("isGrounded", false);
        }

        //apply gravity
        if (!isGrounded){
            verticalSpeed -= gravityFactor * 9.81f * Time.deltaTime;
        }
        else{
            verticalSpeed = 0f;
            currentVerticalSpeed = 0f;
        }

        //set movement vectors to zero
        x = Vector3.zero;
        y = Vector3.zero;
        z = Vector3.zero;

        //glide while holding down the jump button
        if (!isGrounded && Input.GetButton("Jump") && verticalSpeed < 0){
            gravityFactor = 0.1f;
            isGliding = true;
        }
        else {
            gravityFactor = maxGravityFactor;
            isGliding = false;
        }

        //apply jump
        if (isGrounded && Input.GetButtonDown("Jump")){
            effortSound.Play(); //play sound effect
            verticalSpeed = jumpSpeed;
            isGrounded = false;
            y = transform.up * verticalSpeed;
            animator.SetTrigger("jump");
        }
        else if (!isGrounded){
            y = transform.up * verticalSpeed;
        }

        //apply falling animation
        if (controller.velocity.y < 0){
            falling = true;
            animator.SetBool("falling", falling);
        }
        else{
            falling = false;
            animator.SetBool("falling", falling);
        }

        //apply x/z movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDir = Vector3.zero;

        //force forward movement while dashing
        if (dashing){
            vertical = 1;
        }

        //get input direction
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //send speed to animator
        animator.SetFloat("speed", direction.magnitude);

        //apply movement relative to camera angle
        if (isGrounded || canAirControl){
            //find direction to move in
            if (direction.magnitude >= 0.1f){
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camPos.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                //rotate the model with movement
                if (!isAttacking) {
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
        }

        //move towards tether point
        if (lineActive){
            //adjust speed
            if (currentLineSpeed >= lineSpeed){
                currentLineSpeed = lineSpeed;
            }
            else {
                currentLineSpeed += (lineSpeed*2) * Time.deltaTime; //max out speed in 0.5 seconds
            }

            //set speed
            speed = currentLineSpeed;

            /*
            if (vertical > 0) {
                gravityFactor = 0f;
                lineTargetDir = marker.transform.position - transform.position;
                lineTargetMove = lineTargetDir.normalized * lineSpeed * Time.deltaTime;
            }
            */
            //break the tether if player comes close to the tether point
            //if ((transform.position - marker.transform.position).magnitude <= 1) {
            //    lineActive = false;
            //    speed = moveSpeed;
            //}
        }
        else {
            //reset wire values
            lineTargetDir = Vector3.zero;
            lineTargetMove = Vector3.zero;
            currentLineSpeed = 0;
        }

        //check for other inputs
        shootLine();
        dash();
        attack();

        //release wire if player is out of stamina
        if (playerStamina.currentStamina <= 0) {
            disableLine();
        }

        //apply knockback
        if (inKnockback){
            knockbackDuration -= Time.deltaTime;
            if (knockbackDuration < 0) {
                inKnockback = false;
                knockbackDuration = maxKnockbackDuration;
                knockback = Vector3.zero;
            }
        }

        //apply movement to controller
        controller.Move((((moveDir.normalized) * speed) + y) * Time.deltaTime + lineTargetMove + (knockback * Time.deltaTime));
    }

    //manage player attacks
    void attack() {
        
        //only allow player to attack if in the air or wiring
        if (lineActive || !isGrounded) {
            //get attack input
            if (Input.GetKeyDown(KeyCode.Q) && !isAttacking){
                attackSound.Play(); //play sound effect

                //set player as attacking and apply animation
                isAttacking = true;
                attackTimer = attackDuration;
                Debug.Log("Attack Q");
                animator.SetTrigger("hurricaneKick");
                animator.SetBool("isAttacking", isAttacking);
            }
        }

        //check if player is attacking
        if (isAttacking) {

            //apply rotation for the animation
            transform.Rotate(new Vector3(0f, spinSpeed*Time.deltaTime, 0f));
            attackTimer -= Time.deltaTime; //decrease timer

            //remove attacking state
            if (attackTimer <= 0) {
                isAttacking = false;
                animator.SetBool("isAttacking", isAttacking);
            }

            //check for colliisons with enemy
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, hurricaneKickRadius, enemyLayer);

            //apply damage to enemies inside the hitbox
            foreach (Collider enemy in hitEnemies){
                Debug.Log("Player hit: " + enemy.name);
                enemy.GetComponent<EnemyHealth>().takeDamage(hurricaneKickDamage * Time.deltaTime);

                //show damage particle effect
                //GameObject particles = GameObject.Instantiate(Resources.Load("HitEffect"), transform.position, Quaternion.identity) as GameObject;
                //Destroy(particles, 0.3f);
            }
        }

    }

    //apply wire
    void shootLine() {

        //check if the player wire is currently active
        if (lineActive){
            //display the wire
            showLineFromTwoPoints(lineStartPos.position, marker.transform.position);
            lineRenderer.enabled = true;
            
            //reset speeds
            verticalSpeed = 0f;
            gravityFactor = 0f;

            //move towards tether point
            Vector3 targetDir = marker.transform.position - transform.position;
            controller.Move(targetDir.normalized * currentLineSpeed * Time.deltaTime);

            //release wire if close enough
            if ((transform.position - marker.transform.position).magnitude <= 0.1 && !isAttacking){
                lineActive = false;
                animator.SetBool("lineActive", lineActive);
                gravityFactor = maxGravityFactor;
                speed = moveSpeed;
            }

            //release wire if the duration has run out
            lineDuration -= Time.deltaTime;
            if (lineDuration <= 0) {
                disableLine();
            }

            //deplete stamina
            playerStamina.useStamina(12f * Time.deltaTime);
        }
        else{
            //hide line
            lineRenderer.enabled = false;
        }

        //reset camera position
        if (Input.GetButtonDown("Fire1") && !lineActive) {
            aimCam.transform.position = mainCam.transform.position;
        }

        //aim the marker
        if (Input.GetButton("Fire1") && !lineActive){

            //display the line
            ShootLineFromTargetPosition(lineStartPos.position, camPos.TransformDirection(Vector3.forward), lineMaxLength);
            lineRenderer.enabled = true;

            //Debug.Log("FIRE 1");
            
            //check if aiming at a wall
            RaycastHit hit;
            if (Physics.Raycast(lineStartPos.position, camPos.TransformDirection(Vector3.forward), out hit, maxLineLength, enemyLayer | groundLayer)){
                
                //display marker
                marker.transform.position = hit.point;
                marker.GetComponent<Renderer>().enabled = true;
                canLine = true; //allow player to wire
                //Debug.Log("Did Hit");

            }
            else{
                //dont allow player to wire
                canLine = false;
                marker.GetComponent<Renderer>().enabled = false;
                //Debug.Log("Did not Hit");
            }

            //freeze player in air while aiming
            y = Vector3.zero;
            moveDir = Vector3.zero;
            lineTargetMove = Vector3.zero;
            //Debug.DrawRay(lineStartPos.position, camPos.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            //switch to first person camera
            mainCam.SetActive(false);
            aimCam.SetActive(true);

        }
        else{
            //remove marker
            marker.GetComponent<Renderer>().enabled = false;
            //lineRenderer.enabled = false;

            //switch to third person cam
            aimCam.SetActive(false);
            mainCam.SetActive(true);
        }

        //start moving towards tether point
        if (Input.GetButtonUp("Fire1") && !lineActive && canLine) {
            wireSound.Play(); //play sound effect

            //reset aerial movement and start the movement
            gravityFactor = 0f;
            lineActive = true;
            animator.SetBool("lineActive", lineActive);
            moveDir = Vector3.zero;
            y = Vector3.zero;
        }

        //cancel the wire
        if (Input.GetKeyDown(KeyCode.S) && lineActive) {
            disableLine();
        }

    }

    //apply dash
    void dash(){
        //check for input and stamina
        if (Input.GetButtonDown("Fire2") && !dashing && playerStamina.currentStamina >= 20){
            effortSound.Play(); //play sound effect

            //start dash and consume stamina
            dashing = true;
            dashDuration = maxDashDuration;
            speed = dashSpeed;
            playerStamina.useStamina(20f);
        }

        //decrease timer
        if (dashing && dashDuration >= 0) {
            dashDuration -= Time.deltaTime;
        }

        //end dash
        if (dashing && dashDuration < 0) {
            dashing = false;
            speed = moveSpeed; //reset speed
        }

    }

    //stop attacking state
    public void resetAttacking() {
        isAttacking = false;
    }

    //release player from the wire
    public void disableLine() {
        //check if the wire is active
        if (lineActive) {
            //reset aerial movement values
            lineActive = false;
            animator.SetBool("lineActive", lineActive);
            speed = moveSpeed;
            gravityFactor = maxGravityFactor;
            lineDuration = maxLineDuration;
        }
    }

    //REFERENCE: https://answers.unity.com/questions/1142318/making-raycast-line-visibld.html
    //shows line renderer from start point to end point
    void ShootLineFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length){
        
        //create a ray in direction of aim
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        //check for collision
        if (Physics.Raycast(ray, out raycastHit, length)){
            endPosition = raycastHit.point;
        }

        //set line renderer points
        lineRenderer.SetPosition(0, targetPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    //adjusts line renderer points
    void showLineFromTwoPoints(Vector3 startPoint, Vector3 endPoint) {

        //set line renderer points
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}
