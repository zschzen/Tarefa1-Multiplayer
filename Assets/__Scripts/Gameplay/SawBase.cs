using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kTools.Pooling;

using DG.Tweening;
using Photon.Realtime;

public class SawBase : ReciveEvents
{

    #region Vars
    [SerializeField] float FadeOutTransitionDuration = 3f;
    [SerializeField] float InitialImmortalityDuration = 2f;
    [SerializeField] new SpriteRenderer renderer;

    int id = -1;
    public int ID
    {
        get => id;

        set
        {
            if (id < 0) id = value;
        }
    }
    #endregion

    private new void OnEnable()
    {
        base.OnEnable();

        var initialPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        initialPos.z = 0f;
        transform.position = initialPos;

        renderer.material.DOFade(0f, 0f);

        var sequence = DOTween.Sequence();
 
        sequence.AppendCallback(() =>
        {
            GetComponent<Collider2D>().enabled = false;
            gameObject.isStatic = true;
        });
        sequence.Join(renderer.material.DOFade(.25f, 1f));
        sequence.AppendInterval(InitialImmortalityDuration);
        sequence.AppendCallback(() =>
        {
            GetComponent<Collider2D>().enabled = true;
            gameObject.isStatic = false;
        });
        sequence.Join(renderer.material.DOFade(1f, .25f));
        sequence.OnComplete(() => Init());
    }

    protected virtual void Init() { }

    private new void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    protected virtual void OnBecameInvisible()
    {
        if (!gameObject.activeSelf) return;
        StartCoroutine(CountToReturnInstance());
    }

    IEnumerator CountToReturnInstance()
    {
        yield return new WaitForSeconds(FadeOutTransitionDuration);

        //PoolingSystem.ReturnInstance(name.Replace("(Clone)", string.Empty), gameObject);
    }
}
