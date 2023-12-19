using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class FinalUIHandler : NetworkBehaviour
{

    private string _getWinner;
    private int _getRedPoint;
    private int _getBluePoint;

    public TextMeshProUGUI redResult;
    public TextMeshProUGUI blueResult;
    public TextMeshProUGUI redPointText;
    public TextMeshProUGUI bluePointText;

    // Start is called before the first frame update
    void Start()
    {
        _getWinner = PlayerPrefs.GetString("winner");
        _getRedPoint = PlayerPrefs.GetInt("redPoint");
        _getBluePoint = PlayerPrefs.GetInt("bluePoitn");

        if(_getWinner == "red")
        {
            redResult.text = "you win!";
            blueResult.text = "you lose";
        }
        else if(_getWinner == "blue")
        {
            redResult.text = "you lose";
            blueResult.text = "you win!";
        }
        else if(_getWinner == "tie")
        {
            redResult.text = "tie";
            blueResult.text = "tie";
        }
        else
        {
            Debug.Log("Error: Cannot get the right result");
            redResult.text = "";
            blueResult.text = "";
        }

        redPointText.text = _getRedPoint.ToString();
        bluePointText.text = _getBluePoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backToMenu()
    {
        // Destroy all

        // Load new Menu Scene
        
    }
}
