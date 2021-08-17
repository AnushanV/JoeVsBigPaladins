using UnityEngine;

public class GameMaster : MonoBehaviour{
    private static GameMaster instance;

    void Awake(){

        //prevent duplicates
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(gameObject);
        }
    }
}
