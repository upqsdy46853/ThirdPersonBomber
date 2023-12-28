using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class ReadyUIHandler : NetworkBehaviour
{
    public TextMeshProUGUI blueTeamMembers;
    public TextMeshProUGUI redTeamMembers;

    private string _redLocalString;
    private string _blueLocalString;

    [Networked]
    private int _blueTeamCount { get; set; }
    [Networked]
    private int _redTeamCount { get; set; }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        // Member List
        _redLocalString = "";
        _blueLocalString = "";
        _blueTeamCount = 0;
        _redTeamCount = 0;
        PlayerState[] allPlayers = GameObject.FindObjectsOfType<PlayerState>();
        foreach(PlayerState player in allPlayers)
        {
            if(player.Team == Color.blue)
            {
                _blueLocalString += player.nickName;
                _blueLocalString += "\n";
                _blueTeamCount += 1;
            }
            else if(player.Team == Color.red)
            {
                _redLocalString += player.nickName;
                _redLocalString += "\n";
                _redTeamCount += 1;
            }
        }
        blueTeamMembers.text = _blueLocalString;
        redTeamMembers.text = _redLocalString;
    }

    public void StartGame()
    {
        Runner.SessionInfo.IsOpen = false;
        GameObject[] gameObjectsToTransfer = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject gameObjectToTransfer in gameObjectsToTransfer)
        {
            DontDestroyOnLoad(gameObjectToTransfer);
            if(gameObjectToTransfer.TryGetComponent<PlayerState>(out var state))
                state.Respawn(0.0f);
        }
        Runner.SetActiveScene("Game");
        enabled = false;
    }

    public int getBlueTeamNum()
    {
        return _blueTeamCount;
    }
    public int getRedTeamNum()
    {
        return _redTeamCount;
    }
}
