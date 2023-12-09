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
    [SerializeField] private NetworkPlayer _playerPrefab;
    private bool _mouseButton0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }


    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){
        if (runner.IsServer){
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)*3,1,0);
            runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){

    }
    public void OnInput(NetworkRunner runner, NetworkInput input){
        NetworkInputData data = new NetworkInputData();
        if (_mouseButton0)
            data.buttons |= NetworkInputData.MOUSEBUTTON1;
        _mouseButton0 = false;

        data.hInput = Input.GetAxis("Horizontal");
        data.vInput = Input.GetAxis("Vertical");
        data.isJump = Input.GetButtonDown("Jump");

        if(NetworkPlayer.Local != null){
            Vector2 viewVector = NetworkPlayer.Local.GetComponent<camControl>().GetViewVector();
            data.xAxis = viewVector.x;
            data.yAxis = viewVector.y;
            Transform hand = NetworkPlayer.Local.transform.Find("body").Find("hand");
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
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}