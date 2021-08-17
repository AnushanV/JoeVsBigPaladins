using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour{

    //stamina values
    public float maxStamina = 100;
    public float currentStamina;
    public StaminaBar staminaBar;
    public float recoveryRate = 5f;

    void Start(){
        //set current stamina to max stamina
        currentStamina = maxStamina;
        staminaBar.setMaxStamina(maxStamina);
    }

    private void Update(){
        //restore stamina slowly
        currentStamina += recoveryRate * Time.deltaTime;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
        staminaBar.setStamina(currentStamina);
    }
    
    //set the stamina value
    public void useStamina(float stamina){
        //decrease player stamina
        currentStamina -= stamina;
        if (currentStamina < 0) {
            currentStamina = 0;
        }
        //set stamina
        staminaBar.setStamina(currentStamina);
    }
}
