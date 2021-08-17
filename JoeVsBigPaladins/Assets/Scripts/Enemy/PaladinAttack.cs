using UnityEngine;

public class PaladinAttack : MonoBehaviour{
    
    //kick properties
    public int kickDamage = 50;
    public float kickRadius = 2f;
    public Transform kickPos;
    public Vector3 kickOffset;

    //sword properties
    public int swordDamage = 100;
    public float swordRadius = 2f;
    public Transform swordPos;
    public Vector3 swordOffset;

    //player
    public GameObject player;
    public LayerMask playerLayer;

    //creates the hitbox for the kick attack
    public void createKickHitbox(){
        Debug.Log("KICK");
        //check a sphere around leg to determine if hit
        if (Physics.CheckSphere(kickPos.position + (transform.forward * kickOffset.x + transform.forward * kickOffset.y + transform.forward * kickOffset.z), kickRadius, playerLayer)) {
            
            //deal damage and knockback to player
            player.GetComponent<PlayerHealth>().takeDamage(kickDamage);
            applyKnockback(8f, transform.forward);
        }
    }

    //creates a hitbox for the sword attack
    public void createSwordHitbox(){
        Debug.Log("SWORD");
        //check a sphere around the sword to determine if hit
        if (Physics.CheckSphere(swordPos.position + (transform.forward * swordOffset.x + transform.forward * swordOffset.y + transform.forward * swordOffset.z), swordRadius, playerLayer)){

            //deal damage and knockback to player
            player.GetComponent<PlayerHealth>().takeDamage(swordDamage);
            applyKnockback(10f, (transform.forward + transform.up).normalized);
        }
    }

    //apply knockback to the player
    void applyKnockback(float knockback, Vector3 dir) {

        //set knockback values
        player.GetComponent<ThirdPersonMovement>().knockback = dir * knockback;
        player.GetComponent<ThirdPersonMovement>().inKnockback = true;
        player.GetComponent<ThirdPersonMovement>().verticalSpeed = 10f;
        player.GetComponent<ThirdPersonMovement>().y = player.transform.up * 10f;
        //release player tether
        player.GetComponent<ThirdPersonMovement>().disableLine();
    }

    //visualize hitboxes using gizmos
    void OnDrawGizmosSelected(){
        //kick hitbox visualization
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(kickPos.position + (transform.forward * kickOffset.x + transform.forward * kickOffset.y + transform.forward * kickOffset.z), kickRadius);
        //sword hitbox visualization
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(swordPos.position + (transform.forward * swordOffset.x + transform.forward * swordOffset.y + transform.forward * swordOffset.z), swordRadius);
    }

}
