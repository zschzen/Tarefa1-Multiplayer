using UnityEngine;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class ImmortalSaw : SawBase
{
    float spawnY, spawnX;
    Vector2 RandomScreenPoint
    {
        get
        {
            spawnY = Random.Range(
                Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y,
                Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            spawnX = Random.Range(
                Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x,
                Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            return new Vector2(spawnX, spawnY);
        }
    }

    // Event
    public static readonly byte MoveImmortalSawCode = 30;

    protected override void Init() => SendImmortalSawEvent();

    private void GoToTargetPoint(Vector2 newPoint, float randomDuration)
    {
        transform.DOMove(
            newPoint,
            randomDuration
        )
        .SetEase(Ease.InOutBack)
        //.SetEase(Ease.Linear)
        .SetAutoKill()
        .OnComplete(() => SendImmortalSawEvent());
    }

    public void SendImmortalSawEvent()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

        object[] content = new object[] { ID, RandomScreenPoint, Random.Range(2.5f, 6f) };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(MoveImmortalSawCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public override void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != MoveImmortalSawCode) return;

        object[] data = (object[])photonEvent.CustomData;

        var SawIDToMove = (int)data[0];

        if (SawIDToMove == this.ID)
            GoToTargetPoint((Vector2)data[1], (float)data[2]);
    }

}