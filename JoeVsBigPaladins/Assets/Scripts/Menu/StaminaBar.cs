using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour{

    public Slider slider;

    //set max slider values
    public void setMaxStamina(float maxStamina) {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
    }

    //change slider values
    public void setStamina(float stamina) {
        slider.value = stamina;
    }
}
