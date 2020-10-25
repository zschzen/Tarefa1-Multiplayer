using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField nameInputField;
    [SerializeField] Button loginButton;

    public override void OnConnected()
    {
        nameInputField.interactable = true;
        loginButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        nameInputField.interactable = false;
        loginButton.interactable = false;
    }
    public void SetName(string newName)
    {
        if (string.IsNullOrEmpty(newName)) return;

        PhotonNetwork.LocalPlayer.NickName = newName.Trim();
    }

    public void GoToScene(string sceneName)
    {
        // ??
        if (string.IsNullOrEmpty(sceneName) ||
            string.IsNullOrEmpty(nameInputField.text) ||
            string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName)) return;

        MenuManager.Instance?.TransitionToScreenByName(sceneName.Trim());
    }
}
