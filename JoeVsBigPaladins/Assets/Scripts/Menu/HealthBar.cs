using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour{

    public Slider slider;

    //sets the max slider values
    public void setMaxHealth(int maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    //change health value on slider
    public void setHealth(int health) {
        slider.value = health;
    }
}
