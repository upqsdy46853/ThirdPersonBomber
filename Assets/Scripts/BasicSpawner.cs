using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using Cinemachine;


public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    private NetworkRunner _runner;
    [SerializeField] private PlayerState _playerPrefab;
    private bool _mouseButton0;
    private bool _isJump;
    private bool _isThrow;

    public ReadyUIHandler readyUIHandler;


    // Update is called once per frame
    void Update()
    {
        _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
        if(Input.GetButtonDown("Jump"))
            _isJump = true;
        if (Input.GetKeyDown(KeyCode.E))
            _isThrow = true;
    }

    async void JoinRoom(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Game",
            Scene = SceneUtility.GetBuildIndexByScenePath("Scenes/Ready"),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnJoinRoom(){
        JoinRoom(GameMode.AutoHostOrClient);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){

        if (!readyUIHandler)
        {
            readyUIHandler = FindObjectOfType<ReadyUIHandler>(true);
        }

        if (runner.IsServer){
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)*3,1,0);
            int id = player.PlayerId;
            
            PlayerState networkPlayer = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player, (runner, spawnedPlayer)=>{});
        }
    }
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){
        
    }
    public void OnInput(NetworkRunner runner, NetworkInput input){
            
        NetworkInputData data = new NetworkInputData();
        if (_mouseButton0)
            data.buttons |= NetworkInputData.MOUSEBUTTON1;
        _mouseButton0 = false;
        data.isJump = _isJump;
        _isJump = false;
        data.isThrow = _isThrow;
        _isThrow = false;

        data.hInput = Input.GetAxis("Horizontal");
        data.vInput = Input.GetAxis("Vertical");
        data.isJump = Input.GetKey(KeyCode.Space);

        // NetworkPlayer.Local is a static variable used to get the gameObject of local player 
        if(PlayerState.Local != null){
            Vector2 moveVector = PlayerState.Local.GetComponent<movement>().GetMovementVector();
            data.hInput = moveVector.x;
            data.vInput = moveVector.y;

            Vector2 viewVector = PlayerState.Local.GetComponent<camControl>().GetViewVector();
            data.xAxis = viewVector.x;
            data.yAxis = viewVector.y;

            Transform hand = PlayerState.Local.transform.Find("body").Find("hand");
            data.startingVelocity = hand.GetComponent<trajectory>().GetStartingVelocity();
        }

        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {
        Debug.Log("OnHostMigration");
        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        // StartHostMigration

    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    
}
