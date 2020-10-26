using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ReciveEvents : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public virtual void OnEvent(EventData photonEvent) {}
}
