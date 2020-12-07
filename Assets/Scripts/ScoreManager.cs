using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; //For Text Element

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text eggsCaughtText;
    public Text gemsCaughtText;
    public Text spikesAvoidedText;
    public Player player;

    private int score;
    private int eggsCaught;
    private int gemsCaught;
    private int spikesCaught;

    void Awake() //Resets the score everytime this object is enabled again
    {
        score = 0;
        UpdateScoreDisplay(); //Prime the score

        eggsCaught = 0;
        gemsCaught = 0;
        spikesCaught = 0;
    }

    public void UpdateScore(string tag)
    {
        if (tag == "Egg")
        {
            eggsCaught++;
            score++;
        }
        else if (tag == "Gem")
        {
            gemsCaught++;
            score += 3;
        }
        else if (tag == "Spike")
        {
            spikesCaught++;
            score -= 5;
        }
        else
        {
            Debug.Log("Error!  Caught something that's wasn't expected");
        }

        UpdateScoreDisplay();
        UpdateResultsDisplay(); //todo: move this to somewhere that makes sense (UI manager when activating the results UI)
    }

    private void UpdateScoreDisplay()
    {
        //score = Mathf.Clamp(score, 0, 999999); //Limits the score betweeen 0 and 999999 (no nagative values or overflow)
        string scoreFormatted = "";

        //Fills in the rest of the score with zeros (depending on values)
        if (score < 0) //If the score is negative just return zeros so it doesn't look funky
        {
            scoreFormatted = "000000";
        }
        else if (score > 99999)
        {
            scoreFormatted = score.ToString();
        }
        else if (score > 9999)
        {
            scoreFormatted = "0" + score.ToString();
        }
        else if (score > 999)
        {
            scoreFormatted = "00" + score.ToString();
        }
        else if (score > 99)
        {
            scoreFormatted = "000" + score.ToString();
        }
        else if (score > 9)
        {
            scoreFormatted = "0000" + score.ToString();
        }
        else //if (score >= 0)
        {
            scoreFormatted = "00000" + score.ToString();
        }
        scoreText.text = "Score: " + scoreFormatted;
    }

    public void UpdateResultsDisplay()
    {
        int totalEggs = player.GetTotalEggs();
        int totalGems = player.GetTotalGems();
        int totalSpikes = player.GetTotalSpikes();

        eggsCaughtText.text = "Eggs Caught: " + eggsCaught + " of " + totalEggs;
        gemsCaughtText.text = "Gems Caught: " + gemsCaught + " of " + totalGems;
        spikesAvoidedText.text = "Spikes Avoided: " + (totalSpikes - spikesCaught) + " of " + totalSpikes;
    }
}
