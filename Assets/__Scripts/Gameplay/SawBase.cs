using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kTools.Pooling;

using DG.Tweening;


public class SawBase : MonoBehaviour
{
    [SerializeField] float FadeOutTransitionDuration = 3f;
    [SerializeField] float InitialImmortalityDuration = 2f;
    [SerializeField] SpriteRenderer renderer;

    private void OnEnable()
    {
        var initialPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        initialPos.z = 0f;
        transform.position = initialPos;

        renderer.material.DOFade(0f, 0f);

        var sequence = DOTween.Sequence();
        //sequence.Append(transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), .0f));
        sequence.AppendCallback(() =>
        {
            GetComponent<Collider2D>().enabled = false;
            gameObject.isStatic = true;
        });
        sequence.Append(renderer.material.DOFade(.25f, 1f));
        sequence.AppendInterval(InitialImmortalityDuration);
        sequence.Append(renderer.material.DOFade(1f, .25f));
        sequence.AppendCallback(() =>
        {
            GetComponent<Collider2D>().enabled = true;
            gameObject.isStatic = false;
        });
        sequence.OnComplete(() => Init());
    }

    protected virtual void Init() { }

    private void OnDisable()
    {
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

        PoolingSystem.ReturnInstance(name.Replace("(Clone)", string.Empty), gameObject);
    }
}
