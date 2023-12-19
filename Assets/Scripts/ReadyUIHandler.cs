using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class ReadyUIHandler : NetworkBehaviour
{
    public TextMeshProUGUI blueTeamMembers;
    public TextMeshProUGUI redTeamMembers;

    [Networked(OnChanged = nameof(OnListChanged))]
    public NetworkString<_16> redTeamString { get; set; }
    public NetworkString<_16> blueTeamString { get; set; }

    private string _redLocalString;
    private string _blueLocalString;

    // public Dictionary<PlayerRef, PlayerState> RedTeamList = new Dictionary<PlayerRef, PlayerState>();
    // public Dictionary<PlayerRef, PlayerState> BlueTeamList = new Dictionary<PlayerRef, PlayerState>();

    private Dictionary<PlayerRef, PlayerState> _allPlayerList;
    // Start is called before the first frame update
    static void OnListChanged(Changed<ReadyUIHandler> changed)
    {
        changed.Behaviour.blueTeamMembers.text = changed.Behaviour.blueTeamString.ToString();
        changed.Behaviour.redTeamMembers.text = changed.Behaviour.redTeamString.ToString();
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
            if(gameObjectToTransfer.TryGetComponent<PlayerState>(out var state))
                state.Respawn();
        }
        Runner.SetActiveScene("Game");
        enabled = false;
    }

    public void UpdateMembers(Dictionary<PlayerRef, PlayerState> RedTeamList, Dictionary<PlayerRef, PlayerState> BlueTeamList)
    {

        _redLocalString = "";
        _blueLocalString = "";
        foreach (KeyValuePair<PlayerRef, PlayerState> entry in RedTeamList)
        {
            _redLocalString += entry.Value.nickName;
            _redLocalString += "\n";
        }
        foreach (KeyValuePair<PlayerRef, PlayerState> entry in BlueTeamList)
        {
            _blueLocalString += entry.Value.nickName;
            _blueLocalString += "\n";
        }
        redTeamString = _redLocalString;
        blueTeamString = _blueLocalString;
    }
}
