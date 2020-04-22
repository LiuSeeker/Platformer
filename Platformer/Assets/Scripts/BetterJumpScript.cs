using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumpScript : MonoBehaviour
{
    // https://www.youtube.com/watch?v=7KiK0Aqtmzc

    public float FallMultiplier = 3.5f;
    public float lowJumpMultiplier = 2.5f;
    public float maxFallSpeed = -30f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
            if (rb.velocity.y <= maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);

            }
        }
        else if ( rb.velocity.y > 0 && ((!Input.GetKey("space")) && (!Input.GetButton("Fire2"))) ){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
