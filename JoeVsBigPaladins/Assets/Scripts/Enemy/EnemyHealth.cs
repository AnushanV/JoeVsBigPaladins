using UnityEngine;

public class EnemyHealth : MonoBehaviour{
    
    //main health
    public float maxHealth = 300f;
    public float health = 300f;

    //health for legs1
    public float maxLegHealth = 100f;
    public float legHealth = 100f;

    public Animator animator;
    
    bool isKneeling = false;
    
    [SerializeField] GameObject player;
    [SerializeField] private GameObject keyPrefab;


    void Start(){
        //set health values
        health = maxHealth;
        legHealth = maxLegHealth;
    }

    private void Update(){
        //kill paladin when it is out of bounds
        if (transform.position.y < -10) {
            takeDamage(maxHealth);
        }
    }

    //applies damage to the paladin body or legs
    public void takeDamage(float damage){

        //find relative height of player
        float heightDiff = player.transform.position.y - transform.position.y;
        
        //determine if player is attacking leg
        bool isLeg = true;
        if (heightDiff >= 5 || isKneeling){
            isLeg = false;
        }

        //show hit particle effect
        GameObject particles = GameObject.Instantiate(Resources.Load("HitEffect"), (transform.position + heightDiff * Vector3.up), Quaternion.identity) as GameObject;
        Destroy(particles, 0.3f);

        health -= damage;

        //destroy enemy if they have less than 0 health
        if (health <= 0){

            //set offset for key position
            float keyYMod = 2f;
            if (isKneeling) {
                keyYMod = 5f;
            }

            //instantiate a key upon death
            GameObject.Instantiate(keyPrefab, new Vector3(transform.position.x, keyYMod, transform.position.z), Quaternion.identity);
            
            Destroy(gameObject);
        }

        //apply damage to the leg
        if (isLeg){
            legHealth -= damage;
            //make paladin kneel if they take enough leg damage
            if (legHealth <= 0){
                legHealth = 0;
                animator.SetTrigger("kneel");
                isKneeling = true;
                gameObject.GetComponent<EnemyMovement>().isKneeling = isKneeling;
            }
        }
    }

}
