using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private bool leftDown = false;
    private bool rightDown = false;
    private int score = 0;
    private AudioSource mySound;
    private Vector3 leftPos = new Vector3(-1.75f, .59f, 0);
    private Vector3 centerPos = new Vector3(0, .59f, 0);
    private Vector3 rightPos = new Vector3(1.75f, .59f, 0);
    public AudioClip eggCaught;
    public AudioClip gemCaught;
    public AudioClip spikeyBallCaught;
    public Text scoreText;
    public Button pauseButton;

    void Start()
    {
        mySound = GetComponent<AudioSource>(); //Link the variable to the audio component
        updateScore(); //Prime the score
        Debug.Log(Screen.width);
    }

    float test = 0;
    void Update()
    {
        test = Input.GetAxis("WinControllerLeft");
        Debug.Log(test);

        if (Input.GetMouseButton(0))
        {
            Debug.Log(Input.mousePosition.x);
        }
        //Touch-Mouse Controls
        //Add some tweeks here so that the keyboard multi inputs don't mess you up.

        //Keyboard and mouse controls
        if (Input.GetButtonDown("Left") || Input.GetButtonDown("WinControllerLeft") || (Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width / 3))// && EventSystem.current.currentSelectedGameObject == pauseButton))  //!EventSystem.current.IsPointerOverGameObject()
        {
            moveLeft();
        }
        if (Input.GetButtonUp("Left") || Input.GetButtonUp("WinControllerLeft") || Input.GetMouseButtonUp(0))
        {
            if (rightDown == false) //Only move back center if you haven't alrady jumped somewhere else
            {
                transform.position = centerPos;
            }
            else if (Input.GetButton("Right") || Input.GetButton("WinControllerRight")) //Move back right if your still holding down the right input
            {
                moveRight();
            }
            leftDown = false;
        }
        if (Input.GetButtonDown("Right") || Input.GetButtonDown("WinControllerRight") || (Input.GetMouseButton(0) && Input.mousePosition.x > (Screen.width / 3) * 2))
        {
            moveRight();
        }
        if (Input.GetButtonUp("Right") || Input.GetButtonUp("WinControllerRight") || Input.GetMouseButtonUp(0))
        {
            if (leftDown == false) //Only move back center if you haven't alrady jumped somewhere else
            {
                moveCenter();
            }
            else if (Input.GetButton("Left") || Input.GetButton("WinControllerLeft")) //Move back left if your still holding down the left input
            {
                moveLeft();
            }
            rightDown = false;
        }
        if(Input.GetMouseButton(0) && Input.mousePosition.x > (Screen.width / 3) && Input.mousePosition.x < (Screen.width / 3 * 2))
        {
            moveCenter();
        }
    }

    private bool IsPointerOverUIObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void moveLeft()
    {
        leftDown = true;
        transform.position = leftPos;
    }
    private void moveRight()
    {
        rightDown = true;
        transform.position = rightPos;
    }
    private void moveCenter()
    {
        transform.position = centerPos;
    }

    //Catch items
    private void OnTriggerEnter(Collider caughtObject)
    {
       if(caughtObject.gameObject.tag == "Egg")
        {
            score++;
            mySound.clip = eggCaught;
        }
       else if (caughtObject.gameObject.tag == "Gem")
        {
            score += 3;
            mySound.clip = gemCaught;
        }
       else if (caughtObject.gameObject.tag == "Spike")
        {
            score -= 5;
            mySound.clip = spikeyBallCaught;
        }
       else
        {
            Debug.Log("Error!  Caught something that's wasn't expected");
        }
        mySound.Play();
        updateScore();
        Destroy(caughtObject.gameObject);
    }

    public void updateScore(int newScore) //If we pass in a variable update our score and then do the normal stuff
    {
        score = newScore;
        updateScore();
    }
    void updateScore()
    {
        score = Mathf.Clamp(score, 0, 999999); //Limits the score betweeen 0 and 999999 (no nagative values or overflow)
        string scoreWithZeros = "";

        //Fills in the rest of the score with zeros (depending on values)
        if (score > 99999)
        {
            scoreWithZeros = score.ToString();
        }
        else if (score > 9999)
        {
            scoreWithZeros = "0" + score.ToString();
        }
        else if (score > 999)
        {
            scoreWithZeros = "00" + score.ToString();
        }
        else if (score > 99)
        {
            scoreWithZeros = "000" + score.ToString();
        }
        else if (score > 9)
        {
            scoreWithZeros = "0000" + score.ToString();
        }
        else //if (score >= 0)
        {
            scoreWithZeros = "00000" + score.ToString();
        }
        scoreText.text = "Score: " + scoreWithZeros;
    }

    public void Disable()
    {
        transform.position = centerPos;
        leftDown = false;
        rightDown = false;
        this.enabled = false;
    }
}
