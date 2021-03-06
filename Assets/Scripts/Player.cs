﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private bool leftDown = false;
    private bool rightDown = false;
    private AudioSource mySound;
    private Vector3 leftPos = new Vector3(-1.75f, .59f, 0);
    private Vector3 centerPos = new Vector3(0, .59f, 0);
    private Vector3 rightPos = new Vector3(1.75f, .59f, 0);
    public AudioClip eggCaught;
    public AudioClip gemCaught;
    public AudioClip spikeyBallCaught;
    public ScoreManager scoreManager;
    public Button pauseButton;

    void Start()
    {
        mySound = GetComponent<AudioSource>(); //Link the variable to the audio 
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
            mySound.clip = eggCaught;
        }
       else if (caughtObject.gameObject.tag == "Gem")
        {
            mySound.clip = gemCaught;
        }
       else if (caughtObject.gameObject.tag == "Spike")
        {
            mySound.clip = spikeyBallCaught;
        }
       else
        {
            Debug.Log("Error!  Caught something that's wasn't expected");
        }
        mySound.Play();
        scoreManager.UpdateScore(caughtObject.gameObject.tag);
        Destroy(caughtObject.gameObject);
    }

    public void Disable()
    {
        transform.position = centerPos;
        leftDown = false;
        rightDown = false;
        this.enabled = false;
    }

    //Getters
    public int GetTotalEggs() //todo fix this
    {
        //return totalEggs;
        return 0;
    }

    public int GetTotalGems() //todo fix this
    {
        //return totalGems;
        return 0;
    }

    public int GetTotalSpikes() //todo fix this
    {
        //return totalSpikes;
        return 0;
    }
}
