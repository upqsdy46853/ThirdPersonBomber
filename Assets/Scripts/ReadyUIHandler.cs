using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class ReadyUIHandler : NetworkBehaviour
{
    public TextMeshProUGUI blueTeamMembers;
    public TextMeshProUGUI redTeamMembers;

    // public Dictionary<PlayerRef, PlayerState> RedTeamList = new Dictionary<PlayerRef, PlayerState>();
    // public Dictionary<PlayerRef, PlayerState> BlueTeamList = new Dictionary<PlayerRef, PlayerState>();

    private Dictionary<PlayerRef, PlayerState> _allPlayerList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
        // if (_allPlayerList != null)
            // UpdateMembers(_allPlayerList);
    }

    public void StartGame()
    {
        Runner.SessionInfo.IsOpen = false;
        GameObject[] gameObjectsToTransfer = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject gameObjectToTransfer in gameObjectsToTransfer)
        {
            DontDestroyOnLoad(gameObjectToTransfer);
        }
        Runner.SetActiveScene("Game");
        enabled = false;
    }

    public void UpdateMembers(Dictionary<PlayerRef, PlayerState> RedTeamList, Dictionary<PlayerRef, PlayerState> BlueTeamList)
    {


        // _allPlayerList = playerList;

        //blueTeamMembers.text = "";
        //redTeamMembers.text = "";

        //foreach (KeyValuePair<PlayerRef, PlayerState> entry in playerList)
        //{
        //    Color team = entry.Value.Team;
        //    if (playerList.TryGetValue() && team == Color.red)
        //    {
        //        RedTeamList.Add(entry.Key, entry.Value);
        //    }
        //    else if (team == Color.blue)
        //    {
        //        BlueTeamList.Add(entry.Key, entry.Value);
        //    }
        //    else
        //    {
        //        RedTeamList.Add(entry.Key, entry.Value);
        //    }
        //    redTeamMembers.text += entry.Value.nickName;
        //    Debug.Log(entry.Value.nickName);
        //}

        //RedTeamList.Clear();
        //BlueTeamList.Clear();
        redTeamMembers.text = "";
        blueTeamMembers.text = "";
        foreach (KeyValuePair<PlayerRef, PlayerState> entry in RedTeamList)
        {
            redTeamMembers.text += entry.Value.nickName;
            redTeamMembers.text += "\n";
        }
        foreach (KeyValuePair<PlayerRef, PlayerState> entry in BlueTeamList)
        {
            blueTeamMembers.text += entry.Value.nickName;
            blueTeamMembers.text += "\n";
        }
    }
}
