using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNetwork : MonoBehaviourPunCallbacks
{
    private void Awake() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        MenuManager.Instance.TransitionToScreenByName("Login");

        print($"GeneralNetwork :: Conectado ao servidor <b>{PhotonNetwork.CloudRegion}</b>");
    }
}
