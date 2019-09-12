using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    private void Update()
    {

    }

    // Start is called before the first frame update
    public void CloseGame()
    {
        Application.Quit();
    }
}
