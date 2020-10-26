using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform PlayerListDisplay;
    [SerializeField] GameObject PlayerNamePrefab;

    public Dictionary<string, Text> AllPlayers { get; private set; }

    private void Awake()
    {
        AllPlayers = new Dictionary<string, Text>();
    }

    private new void OnEnable()
    {
        base.OnEnable(); // add target Callback

        var PlayerInRoomList = PhotonNetwork.PlayerList;

        Text playerDisplayName = default;
        foreach (var player in PlayerInRoomList)
        {
            CreateNewPlayerDisplayName(ref playerDisplayName, player.NickName);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (AllPlayers.ContainsKey(newPlayer.NickName)) return;

        Text playerDisplayName = default;

        CreateNewPlayerDisplayName(ref playerDisplayName, newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // clear on list
        Destroy(AllPlayers[otherPlayer.NickName].gameObject);
        AllPlayers.Remove(otherPlayer.NickName);
    }

    /// <summary>
    /// Instantiate playerDisplayName, put on AllPLayers dict without alloc
    /// </summary>
    private void CreateNewPlayerDisplayName(ref Text playerNameDisplay,
                                            string newPlayerNickname)
    {
        if (AllPlayers.ContainsKey(newPlayerNickname)) return; // prevent duplication

        playerNameDisplay = Instantiate(PlayerNamePrefab, PlayerListDisplay).GetComponent<Text>();
        playerNameDisplay.text = newPlayerNickname;

        if (PhotonNetwork.LocalPlayer.NickName == newPlayerNickname) playerNameDisplay.color = Color.green;

        AllPlayers.Add(newPlayerNickname, playerNameDisplay);
    }
}
