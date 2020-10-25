using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lerpDuration;
    [SerializeField] bool CanMove = true;
    public Rigidbody2D rb { get; private set; }


    //float timer;
    Vector2 movInput = Vector2.zero;
    Vector2 pos;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;
        //if (!Input.anyKey) return;

        movInput.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!movInput.Equals(Vector2.zero))
        {
            movInput = (movInput.normalized * speed) * Time.fixedDeltaTime;

            //timer = lerpDuration;

            pos = Camera.main.WorldToViewportPoint(rb.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            rb.position = Camera.main.ViewportToWorldPoint(pos);
        }

        rb.velocity = movInput;

        //rb.velocity = Vector2.Lerp(movInput, Vector2.zero, timer);

        //if (timer > .0f) timer -= Time.fixedDeltaTime * lerpDuration;
    }
}
