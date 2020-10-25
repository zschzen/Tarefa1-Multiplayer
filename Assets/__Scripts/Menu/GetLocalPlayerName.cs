using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GetLocalPlayerName : MonoBehaviour
{
    private void OnEnable() => GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
}
