using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBehaviour : MonoBehaviour
{
    public AudioClip eggCracking;
    public AudioClip goldenEggCracking;
    public AudioClip spikeHitting;
    private AudioSource mySound;

    void Start()
    {
        mySound = GetComponent<AudioSource>(); //Link the variable to the audio component
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Egg")
        {
            mySound.clip = eggCracking;
            mySound.Play();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Gem")
        {
            mySound.clip = goldenEggCracking;
            mySound.Play();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Spike")
        {
            mySound.clip = spikeHitting;
            mySound.Play();
            Destroy(collision.gameObject);
        }
    }
}
