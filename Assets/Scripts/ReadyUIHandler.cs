using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;

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

    // Map selecter
    public GameObject mapSelecter;
    public RawImage mapBox0;
    public RawImage mapBox1;
    public RawImage mapBox2;
    public TextMeshProUGUI mapText0;
    public TextMeshProUGUI mapText1;
    public TextMeshProUGUI mapText2;
    private int _selectedMap;

    private void Awake()
    {

    }
    private void Start()
    {
        mapSelecter.SetActive(false);// = false;
    }

    // Update is called once per frame
    void Update()
    //public override void FixedUpdateNetwork()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            RPC_StartGame();
        }

        // Member List
        _redLocalString = "";
        _blueLocalString = "";
        _blueTeamCount = 0;
        _redTeamCount = 0;
        PlayerState[] allPlayers = GameObject.FindObjectsOfType<PlayerState>();
        foreach(PlayerState player in allPlayers)
        {
            if(player.HasStateAuthority && player.HasInputAuthority)
            {
                mapSelecter.SetActive(true);// = true;
            }
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
            if(player.HasInputAuthority)
                player.ChangeState(PlayerState.GameState.gameReady);
        }
        blueTeamMembers.text = _blueLocalString;
        redTeamMembers.text = _redLocalString;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_StartGame()
    {
        Runner.SessionInfo.IsOpen = false;
        GameObject[] gameObjectsToTransfer = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject gameObjectToTransfer in gameObjectsToTransfer)
        {
            DontDestroyOnLoad(gameObjectToTransfer);
            //if(gameObjectToTransfer.TryGetComponent<PlayerState>(out var state)){
            //    if(state.HasInputAuthority)
            //         state.ChangeState(PlayerState.GameState.gameStart);
            //    state.Respawn(0.0f);
            //}
                
        }

        Runner.SetActiveScene(getMapName());

        foreach(GameObject gameObjectToTransfer in gameObjectsToTransfer)
        {
            //DontDestroyOnLoad(gameObjectToTransfer);
            if(gameObjectToTransfer.TryGetComponent<PlayerState>(out var state)){
                if(state.HasInputAuthority)
                    state.ChangeState(PlayerState.GameState.gameStart);
                state.Respawn(0.0f);
            }
                
        }

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
    public void selectMap(int code)
    {
        Color yello = new Color(1.0f, 0.5f, 0.0f);
        Color unselected = Color.black;
        _selectedMap = code;
        if(code == 1)
        {
            mapBox0.color = yello;
            mapText0.color = yello;

            mapBox1.color = unselected;
            mapText1.color = unselected;
            mapBox2.color = unselected;
            mapText2.color = unselected;
        }
        else if(code == 2)
        {
            mapBox1.color = yello;
            mapText1.color = yello;

            mapBox0.color = unselected;
            mapText0.color = unselected;
            mapBox2.color = unselected;
            mapText2.color = unselected;
        }
        else if(code == 3)
        {
            mapBox2.color = yello;
            mapText2.color = yello;

            mapBox1.color = unselected;
            mapText1.color = unselected;
            mapBox0.color = unselected;
            mapText0.color = unselected;
        }
    }
    private string getMapName()
    {
        if (_selectedMap == 1)
            return "Game";
        else if (_selectedMap == 2)
            return "Game_2";
        else
            return "Game_3";
    }
}
