using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggPhysics : MonoBehaviour
{
    public float speed = 5;

    //Movement of egg downwards
    void FixedUpdate()
    {
        transform.Translate(0,-speed * Time.deltaTime, 0);
    }
}