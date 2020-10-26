using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class InitGameplayNetwork : ReciveEvents
{
    public static readonly byte InitGameplayLoadingCode = 1;

    public void SendInitGameplayLoadingEvent()
    {
        object[] content = new object[] { true };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(InitGameplayLoadingCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public override void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode != InitGameplayLoadingCode) return;

        Addressables.LoadSceneAsync("Gameplay").Completed += SceneLoadComplete;
    }

    private void SceneLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) Debug.LogError("Erro ao carergar o Gameplay");

        var CurrentRoom = PhotonNetwork.CurrentRoom;
        CurrentRoom.IsOpen = false;
        CurrentRoom.IsVisible = false;

        Debug.Log("InitGameplayNetwork :: Gameplay carregada");
        Debug.Log("InitGameplayNetwork :: A sala agora está fechada");
    }
}
