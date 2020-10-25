using UnityEngine;
using DG.Tweening;

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

    protected override void Init()
    {
        transform.DOMove(
            RandomScreenPoint,
            Random.Range(3.7f, 5.5f)
        )
        .SetEase(Ease.InOutBack)
        .OnComplete(() => Init());
    }

}