using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class ReadyUIHandler : NetworkBehaviour
{
    public TextMeshProUGUI blueTeamMembers;
    public TextMeshProUGUI redTeamMembers;

    public Dictionary<PlayerRef, PlayerState> ReadTeamList = new Dictionary<PlayerRef, PlayerState>();
    public Dictionary<PlayerRef, PlayerState> BlueTeamList = new Dictionary<PlayerRef, PlayerState>();
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
    }

    public void UpdateMembers(Dictionary<PlayerRef, PlayerState> playerList){
        
        blueTeamMembers.text = "Player1\n";
        redTeamMembers.text = "Player2\n";
    }
}
