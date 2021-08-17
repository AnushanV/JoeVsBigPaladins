[System.Serializable]
public class PlayerProgress{
    public int levelNum; //players current level

    //initialize without parameter
    public PlayerProgress() {
        //default level
        this.levelNum = 1;
    }

    //initialize with parameter
    public PlayerProgress(int levelNum) {
        this.levelNum = levelNum;
    }
}
