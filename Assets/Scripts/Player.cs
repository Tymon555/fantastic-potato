using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public float speed = 0.3f;

    private Rigidbody2D rb2D;
    private bool moving = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            int horizontal = 0;
            int vertical = 0;
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));
            Vector3 end = rb2D.position;
            end.x += horizontal;
            end.y += vertical;
            if (horizontal != 0) vertical = 0;
            if (vertical != 0) horizontal = 0;
            if ((horizontal != 0 || vertical != 0) && !moving)
                StartCoroutine(SmoothMovement(end));
        }
    }

    IEnumerator SmoothMovement(Vector3 end)
    {
        moving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, speed * Time.deltaTime);
            rb2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        moving = false;
    }
}
