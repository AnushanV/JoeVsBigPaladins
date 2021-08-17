using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour{

    //set health values
    public int maxHealth = 200;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject levelLoader;

    [SerializeField] AudioSource damageSound;

    void Start(){
        //get the level loader
        levelLoader = GameObject.Find("LevelLoader");
        
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    private void Update(){
        
        /*
        //test taking damage
        if (Input.GetKeyDown(KeyCode.P)) {
            takeDamage(20);
        }
        */

        //restart current level if r key is pressed
        if (Input.GetKeyDown(KeyCode.R)){
            levelLoader.GetComponent<Fade>().LoadNextScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //apply damage to player health
    public void takeDamage(int damage){
        damageSound.Play(); //play sound effect

        //decrease player health by damage
        currentHealth -= damage;
        Debug.Log("PLAYER: " + currentHealth);
        healthBar.setHealth(currentHealth); //adjust healthbar slider

        //show particle effect
        GameObject particles = GameObject.Instantiate(Resources.Load("PlayerHitEffect"), (transform.position), Quaternion.identity) as GameObject;
        Destroy(particles, 0.3f);

        //kill player and reload scene
        if (currentHealth <= 0) {
            levelLoader.GetComponent<Fade>().LoadNextScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }
    }
}
