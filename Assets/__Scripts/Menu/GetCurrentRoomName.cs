using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCurrentRoomName : MonoBehaviour
{
    private void OnEnable() => GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
}
