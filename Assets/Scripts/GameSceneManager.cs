using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameSceneManager : MonoBehaviour
{
    public Text buttonPressedCountText;
    private int buttonPressedCount;
    private PhotonView photonView;
    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(PhotonNetwork.IsConnected){
            PhotonNetwork.Instantiate("Player", new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0);
        }else{
            // offline mode
        }

    }

    // Update is called once per frame
    void Update()
    {
        // buttonPressedCountText.text = buttonPressedCount.ToString();
    }

    [PunRPC]
    public void buttonPressedRPC(int count){
        buttonPressedCount += count;
    }

    public void buttonPressed(){

        if(PhotonNetwork.IsConnected){
            photonView.RPC("buttonPressedRPC", RpcTarget.All, 1);
        }

    }
}
