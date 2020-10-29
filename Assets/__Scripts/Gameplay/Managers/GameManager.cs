using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kTools.Pooling;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class GameManager : ReciveEvents
{
    [SerializeField] float SawSpawnTime = 10f;
    [SerializeField] GameObject[] SawTypes;

    // Event
    public static readonly byte SpawnSawCode = 10;

    private GameObject player;
    private Dictionary<string, int> AllSawsInstanceCount = new Dictionary<string, int>();
    private Dictionary<string, int> AllSawsInstanceLimit = new Dictionary<string, int>();

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        instance = this;


        foreach (var item in SawTypes)
        {
            //PoolingSystem.CreatePool(item.name, item, SawInstanceCount);
            AllSawsInstanceCount.Add(item.name, 0);
            if (item.name == "ImmortalSaw") AllSawsInstanceLimit.Add(item.name, 10); // get from a scriptable
            else AllSawsInstanceLimit.Add(item.name, int.MaxValue);
        }

        if (PhotonNetwork.IsConnectedAndReady) SpawnPlayers();
        else
        { // offline editor?
            var playerGO = Resources.Load<GameObject>("Player");
            playerGO = GameObject.Instantiate(playerGO, Vector3.zero, Quaternion.identity);
        }
    }

    private void Start()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

        CountdownTimer.OnCountdownTimerHasExpired += SendSpawnSawEvent;
        CountdownTimer.SetStartTime();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (!newMasterClient.IsLocal) return;

        CountdownTimer.OnCountdownTimerHasExpired += SendSpawnSawEvent;
        CountdownTimer.SetStartTime();
    }

    void SpawnSaw(string sawType)
    {
        if (AllSawsInstanceCount[sawType] >= AllSawsInstanceLimit[sawType]) return; // limit saws
        //if (CountdownTimer.instance.TimeRemaining() > .0f) return;

        //var saw = PhotonNetwork.Instantiate(sawType, Vector3.zero, Quaternion.identity);
        var saw = Instantiate(SawTypes[0], Vector3.zero, Quaternion.identity);
        saw.GetComponent<SawBase>().ID = AllSawsInstanceCount[sawType]++;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CountdownTimer.SetStartTime();
            //CountdownTimer.instance.enabled = true;
        }
    }

    void SpawnPlayers()
    {
        if (PlayerController.LocalPlayer != null) return;

        var newPlayer = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        newPlayer.GetComponent<Renderer>().material.color = Color.green;
    }

    public void SendSpawnSawEvent()
    {
        object[] content = new object[] { SawTypes[Random.Range(0, SawTypes.Length - 1)].name };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SpawnSawCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public override void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != SpawnSawCode) return;

        object[] data = (object[])photonEvent.CustomData;

        SpawnSaw((string)data[0]);
    }

}
