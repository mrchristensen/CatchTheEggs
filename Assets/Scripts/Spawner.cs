using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: When you spawn the last egg mark it as the last one so you can fix detecting the end of a game
//TODO: Enums for difficulty?

public class Spawner : MonoBehaviour
{
    public GameObject egg;
    public GameObject gem;
    public GameObject spike;
    public GameObject player;
    public UIManager UI;
    public Transform leftPos;
    public Transform centerPos;
    public Transform rightPos;
    public List<Line> spawnQueue = new List<Line>();
    public float spawnrate = .7f;
    public float EasySpeed = 5f;
    public float NormalSpeed = 6f;
    public float HardSpeed = 7f;
    private int lineNumIndex = 0; //Todo fix this
    private bool reset = false;
    private List<string> usedSpawnSet = new List<string>();
    private const int classicNumSets = 3; //The number of sets to add to the queue for a classic game
    private const int numSets = 3; //The number of all the possible sets that I've hand created
    private string gamemode;
    private string difficulty;

    public void StartGame()
    {
        if (gamemode == "Classic")
        {
            StartClassic(difficulty);
        }
        else if (gamemode == "Endless")
        {
            StartEndless(difficulty);
        }
        else
        {
            Debug.Log("Unexpected mode");
        }
    }

    void StartClassic(string difficulty)
    {
        FillSpawnQueue(difficulty, classicNumSets);
        InvokeRepeating("SpawnLine", 0, spawnrate);
    }

    void StartEndless(string difficulty)  //Public to be able to recieve the message from the button
    {
        //UpdateSpeed();
        FillSpawnQueue("Normal", 3); //Todo Fix this
        InvokeRepeating("SpawnLine", 0, spawnrate); //Todo: there need to be a way to increase spawn rates
    }

    void FillSpawnQueue(string difficulty, int numSets) //Fills the Queue with x number of sets | x = numSets
    {
        for (int i = 0; i < numSets; i++)
        {
            int setID;
            do
            {
                setID = Random.Range(0, Spawner.numSets);
            }
            while (usedSpawnSet.Contains(difficulty + setID)); //If we've already used that set try again
            //Todo: if this is used for endless then we need to reset the usedSpawnSet after everything has been used.
            //Todo: Maybe a better sollution would be adding all the sets to a set, and then getting a random one from that set

            Debug.Log("Fill spawn queue with setID: " + setID + ", on difficulty: " + difficulty);
            SendMessage(difficulty + setID);  //ie calls Easy0() in this script
            usedSpawnSet.Add(difficulty + setID);

        }
    }

    void GenerateSpawnSet() //Makes all the strings in the list so that you can pick from which one to spawn in (Use this to generate a new set, or to refill an empty set)
    {

    }

    void UpdateSpeed(float speed)
    {
        Debug.Log("Speed: "+ speed);
        egg.GetComponent<EggPhysics>().speed = speed;
        gem.GetComponent<EggPhysics>().speed = speed;
        spike.GetComponent<EggPhysics>().speed = speed;
    }

    //This function will check to see if the game should be ended - is called once the spawn queue has been emptied
    void ResetGame()
    {
        if (GameObject.FindGameObjectWithTag("Egg") == false && GameObject.FindGameObjectWithTag("Gem") == false && GameObject.FindGameObjectWithTag("Spike") == false)
        {
            CancelInvoke();
            spawnQueue.Clear();
            usedSpawnSet.Clear();
            lineNumIndex = 0; //Resests the index for next game rotation
            UI.MainMenuGUI();
            player.GetComponent<Player>().Disable(); //Revoke controls from the player (and center them and reset bool vars)
            Debug.Log("Game Over");
        }
        else
        {
            Debug.Log("Nope, not ready to restart, try again");
            Invoke("ResetGame", 0); //If there are still items to catch check again in 0 seconds
        }
    }

    //Only called if you click quit while paused
    public void QuitGame()
    {
        EggPhysics[] catchables = GameObject.FindObjectsOfType<EggPhysics>();
        foreach (EggPhysics myObject in catchables)
        {
            GameObject.Destroy(myObject.gameObject);
        }
        Time.timeScale = 1; //Because it's currently 0 if we're paused here
        ResetGame();
        Debug.Log("Game Quit");
    }

    void SpawnLine()
    {
        if (lineNumIndex >= spawnQueue.Count) //If you've gone through the whole spawn queue) - End of Game
        {
            CancelInvoke();
            ResetGame();
            return;
        }
        if (spawnQueue[lineNumIndex].left != Spawnables.None) //If we need to spawn something
        {
            SpawnIn(spawnQueue[lineNumIndex].left, leftPos); //Do it (at the possition we checked for - left, right, or center)
        }
        if (spawnQueue[lineNumIndex].center != Spawnables.None)
        {
            SpawnIn(spawnQueue[lineNumIndex].center, centerPos);
        }
        if (spawnQueue[lineNumIndex].right != Spawnables.None)
        {
            SpawnIn(spawnQueue[lineNumIndex].right, rightPos);
        }
        lineNumIndex++;
    }

    void SpawnIn(Spawnables myObject, Transform myLocation)
    {
        if (myObject == Spawnables.Egg)
        {
            Instantiate(egg, myLocation);
        }
        else if (myObject == Spawnables.Gem)
        {
            Instantiate(gem, myLocation);
        }
        else if (myObject == Spawnables.Spike)
        {
            Instantiate(spike, myLocation);
        }
    }

    public void SetGamemode(string newGamemode)
    {
        gamemode = newGamemode;
    }

    public void SetDifficulty(string newDifficulty)
    {
        float speed = 0;
        difficulty = newDifficulty;

        if (difficulty == "Easy")
        {
            speed = EasySpeed;
        }
        else if (difficulty == "Normal")
        {
            speed = NormalSpeed;
        }
        else if (difficulty == "Hard")
        {
            speed = HardSpeed;
        }
        else
        {
            Debug.Log("Unexpected Difficulty");
        }

        UpdateSpeed(speed);
    }

    //spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
    //Egg Spike None Gem
    //%%% If you're going to add empty lines as buffers it's going to cause issues: if it's at the bigginging then you'll have a delayed game start.  If it's at the end then the game will end with a delay (unless we are going to detect the end of the game differently, IE the 100th egg hits the ground or is caught)
    void Easy0()
    {
        Debug.Log("Easy0");
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
    }
    void Easy1()
    {
        Debug.Log("Easy1");
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
    }
    void Easy2()
    {
        Debug.Log("Easy2");
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
    }
    void Easy3()
    {
        Debug.Log("Easy3");
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
    }
    void Normal0()
    {
        Debug.Log("Normal0");
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Spike, Spawnables.Egg, Spawnables.Spike));
        spawnQueue.Add(new Line(Spawnables.Gem, Spawnables.Spike, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Spike, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.Spike));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Spike, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.Spike, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Spike, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.Spike, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.None, Spawnables.Egg));
        spawnQueue.Add(new Line(Spawnables.None, Spawnables.Egg, Spawnables.None));
        spawnQueue.Add(new Line(Spawnables.Egg, Spawnables.None, Spawnables.None));
    }
    void Normal1()
    {

    }
    void Normal2()
    {

    }
    void Normal3()
    {

    }
    void Normal4()
    {

    }
    void Hard1()
    {

    }
    void Hard2()
    {

    }
    void Hard3()
    {

    }

}
