using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Text playerCount;
    
    void Update(){
        if(PhotonNetwork.CurrentRoom != null){
            playerCount.text = "Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }

    public void StartGame(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
