using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private int avgFrameRate;
    private Text display_Text;

    private void Start()
    {
        display_Text = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = string.Format("FPS: {0}", avgFrameRate.ToString("000"));
    }
}
