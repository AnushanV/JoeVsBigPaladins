using UnityEngine;
using UnityEngine.UI;

public class UpdateKeys : MonoBehaviour{
    
    public int numKeys = 0; //current number of keys
    public int maxKeys = 3; //keys needed to advance

    [SerializeField] private string keyText;
    [SerializeField] private Text textField;
    

    void Start(){
        //set key message
        keyText = "Keys: 0 / " + maxKeys;
        textField.text = keyText;
    }

    //increments the number of keys
    public void addKey() {

        numKeys++;

        //cap keys at max keys
        if (numKeys > maxKeys) {
            numKeys = maxKeys;
        }

        //edit key text
        keyText = "Keys: " + numKeys + " / " + maxKeys;
        textField.text = keyText;
    }
}
