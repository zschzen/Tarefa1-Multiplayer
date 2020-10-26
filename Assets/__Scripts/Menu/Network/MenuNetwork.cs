using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] byte RoomSize;

    public void JoinRoom() => PhotonNetwork.JoinRandomRoom(null, 0);

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.TransitionToScreenByName("Lobby");

#if UNITY_EDITOR
        Debug.Log($"MenuNetwork :: Entrando na sala '{PhotonNetwork.CurrentRoom.Name}'");
#endif
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();

#if UNITY_EDITOR
        Debug.LogWarning("Falha ao criar sala. Tentando criar outra...");
        Debug.LogWarning($"Mensagem: {message}");
#endif
    }

    private void CreateRoom()
    {
        var roomName = System.DateTime.Now.Ticks.ToString("x");
        roomName = roomName.Substring(roomName.Length - 4, 4);

        RoomOptions roomOpt = new RoomOptions
        {
            MaxPlayers = RoomSize,
            IsOpen = true,
            IsVisible = true
        };

        PhotonNetwork.CreateRoom(roomName, roomOpt);
#if UNITY_EDITOR
        Debug.Log($"MenuNetwork :: Criando a sala '{roomName}'");
#endif
    }
}
