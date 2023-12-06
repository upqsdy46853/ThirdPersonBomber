using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class PlayerInitializeScript : MonoBehaviour
{
    private PhotonView photonView;
    public GameObject CM_vcam;
    private GameObject camLookAt;
    private GameObject hand;
    private GameObject landingPoint;
    void Start()
    {
        camLookAt = GameObject.Find("camLookAt");
        hand = GameObject.Find("hand");
        landingPoint = GameObject.Find("landingPoint");

        photonView = GetComponent<PhotonView>();
		if(photonView != null){
			if(PhotonNetwork.IsConnected == true && photonView.IsMine)
			{
                hand.name = "my_hand";
                landingPoint.name = "my_land";
				GameObject playerCamera = Instantiate(CM_vcam, new Vector3(0, 0, 0), Quaternion.identity);
				playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = camLookAt.transform;
				playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = camLookAt.transform;

                GetComponent<movement>().enabled = true;
                GetComponent<camControl>().enabled = true;
                hand.SetActive(true);
                landingPoint.SetActive(true);

			}
            else if(PhotonNetwork.IsConnected == true && !photonView.IsMine){
                hand.name = "other_hand";
                landingPoint.name = "other_land";
                GetComponent<movement>().enabled = false;
                GetComponent<camControl>().enabled = false;
                hand.SetActive(false);
                landingPoint.SetActive(false);

            }
		}
    }

}
