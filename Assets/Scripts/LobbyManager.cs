using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime; 

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text status;
    

    void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
        if(!PhotonNetwork.IsConnected){
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Matchmaking method 1 : Quick Match (JoinRandomOrCreateRoom)
    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    // Matchmaking method 2 : Create/Join With Room Name
    public InputField createRoomNameInput;
    public InputField joinRoomNameInput; 
    public byte maxPlayersPerRoom = 5;

    public void CreateRoom(){
        if(createRoomNameInput.text != ""){
            PhotonNetwork.CreateRoom(createRoomNameInput.text, new RoomOptions {
                MaxPlayers = maxPlayersPerRoom, 
                IsVisible = true 
            });
        }
    }

    public void JoinRoom(){
        if(joinRoomNameInput.text != ""){
            PhotonNetwork.JoinRoom(joinRoomNameInput.text);
        }
    }


    // Callbacks
    public override void OnConnectedToMaster(){
        status.text = "Connected To Master Server";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby(){
        status.text = "Joined Lobby";
    } 

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("RoomScene");
    }

    // Room List (only visible in lobby)
    public Text roomListText;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomListText.text = "";
        Debug.Log("Room List Updated");
        for(int i=0; i<roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            roomListText.text = roomListText.text + info.Name + "\n";
        }
    }


}
