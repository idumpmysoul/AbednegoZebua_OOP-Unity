using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Vector2 maxSpeed = new Vector2(7, 5); // Kecepatan maksimum untuk X dan Y
    [SerializeField] private Vector2 timeToFullSpeed = new Vector2(1, 1); // Waktu untuk mencapai kecepatan maksimum
    [SerializeField] private Vector2 timeToStop = new Vector2(0.5f, 0.5f); // Waktu untuk berhenti
    [SerializeField] private Vector2 stopClamp = new Vector2(2.5f, 2.5f); // Batas kecepatan minimum sebelum berhenti

    private Vector2 moveDirection;
    private Vector2 moveVelocity;
    private Vector2 moveFriction;
    private Vector2 stopFriction;
    private SpriteRenderer spriterenderer;

    private Vector2 spriteSize = Vector2.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Menghitung Starting value
        moveVelocity.x = (2 * maxSpeed.x) / timeToFullSpeed.x;
        moveVelocity.y = (2 * maxSpeed.y) / timeToFullSpeed.y;
        moveFriction.x = (-2 * maxSpeed.x) / Mathf.Pow(timeToFullSpeed.x, 2);
        moveFriction.y = (-2 * maxSpeed.y) / Mathf.Pow(timeToFullSpeed.y, 2);
        stopFriction.x = (-2 * maxSpeed.x) / Mathf.Pow(timeToStop.x, 2);
        stopFriction.y = (-2 * maxSpeed.y) / Mathf.Pow(timeToStop.y, 2);
        spriterenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //Input dari keyboard
        float x_input = Input.GetAxis("Horizontal");
        float y_input = Input.GetAxis("Vertical");
        moveDirection = new Vector2(x_input, y_input).normalized;

        if (timeToFullSpeed.x > 0 && timeToFullSpeed.y > 0)
        {
            moveVelocity = new Vector2((moveDirection.x * maxSpeed.x) / timeToFullSpeed.x, (moveDirection.y * maxSpeed.y) / timeToFullSpeed.y);
        }
        else
        {
            moveVelocity = Vector2.zero;
        }

        if (moveDirection.magnitude == 0)
        {
            moveVelocity = Vector2.MoveTowards(moveVelocity, Vector2.zero, GetFriction().magnitude * Time.deltaTime);
        }

        if (moveVelocity.magnitude < stopClamp.magnitude)
        {
            moveVelocity = Vector2.MoveTowards(moveVelocity, Vector2.zero, GetFriction().magnitude * Time.deltaTime);
        }

        if (!float.IsNaN(moveVelocity.x) && !float.IsNaN(moveVelocity.y))
        {
            rb.velocity = moveVelocity;
        }

        transform.position = MoveBound();
    }

    private Vector2 GetFriction()
    {
        // Mengembalikan nilai gesekan berdasarkan status gerakan
        return moveDirection != Vector2.zero ? 
            new Vector2(moveFriction.x, moveFriction.y) : 
            new Vector2(stopFriction.x, stopFriction.y);
    }
    //move friction, stop friction
    //move friction --> dari bergerak ke bergerak lanjut, atau diam
    //stop friction --> dari diam ke bergerak
    public bool IsMoving()
    {
        if (rb.velocity.magnitude > 0) return true;
        else return false;
    }

    public Vector2 MoveBound()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2(0,0));
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2(1,1));
        spriteSize.x = spriterenderer.bounds.size.x * 1.15f;
        spriteSize.y = spriterenderer.bounds.size.y * 1.15f;
        var adjust = 0.14f;
        var adjustAll = 0.2f;
        if (IsMoving()){
            max.x = max.x - spriteSize.x;
            min.x = min.x + spriteSize.x;

            max.y = max.y - spriteSize.y - adjustAll;
            min.y = min.y + spriteSize.y;
        }
        else {
            max.x = max.x - spriteSize.x + adjust;
            min.x = min.x + spriteSize.x - adjust;

            max.y = max.y - spriteSize.y + adjust + adjustAll;
            min.y = min.y + spriteSize.y - adjust;
        }

        return new Vector2(
            Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y)
        );
    }

}
