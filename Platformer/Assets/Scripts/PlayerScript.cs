using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // https://www.youtube.com/watch?v=STyY26a_dPY

    public float walkSpeed = 10;
    public float jumpSpeed = 10;
    public float coyoteTime = 0.1f;
    public float slideSpeed = -3.5f;
    public float wallJumpLerp = 10;
    public float shootTime = 1f;

    public float FallMultiplier = 3.5f;
    public float lowJumpMultiplier = 2.5f;
    public float maxFallSpeed = -17f;

    public bool canJump = true;
    public bool groundCheck = true;

    public bool wallJumped = false;
    public bool canWallJump = true;

    public bool canMove = true;

    public bool faceRight = true;

    public bool canShoot = true;
    public bool lookingUp = false;
    public bool lookingDown = false;

    public bool owplatformActive = true;

    private Rigidbody2D rb;
    private PlayerCollision coll;

    public GameObject bullet;

    private GameObject[] owplatforms2;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<PlayerCollision>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        /* Movimento absoluto
        if (x > 0)
        {
            x = 1f;
        }
        else if (x < 0){
            x = -1f;
        }
        if (y > 0)
        {
            y = 1f;
        }
        else if (y < 0){
            y = -1f;
        }
        */

        Vector2 dir = new Vector2(x, y);

        if (Input.GetKey("k") || Input.GetButton("Fire3"))
        {
            if (canShoot)
            {
                Shoot();
            }
        }

        LookingAt(dir);

        Walk(dir);

        if (coll.onSlope && dir.x == 0)
        {
            Physics2D.gravity = new Vector2(0, 0);
        }
        else
        {
            Physics2D.gravity = new Vector2(0, -9.8f);
        }

        if (coll.onGroundCenter || coll.onSlope)
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isWall", false);
            anim.SetBool("isWalljumping", false);
            if (groundCheck)
            {
                canJump = true;
            }
            canWallJump = true;
        }
        else if (!(coll.onGroundCenter || coll.onGroundEdge))
        {
            StartCoroutine(CoiyoteFunc());
            anim.SetBool("isWall", coll.onWall);
        }

        Fall(dir);

        if(!owplatformActive && y >= 0)
        {
            owplatformActive = true;
            for (int i = 0; i < owplatforms2.Length; i++)
            {
                owplatforms2[i].SetActive(true);
            }
        }

        else if(coll.onOWPlatform && (Input.GetKeyDown("space") || Input.GetButtonDown("Fire2")) && y < 0)
        {
            owplatformActive = false;
            canJump = false;
            GameObject[] owplatforms = GameObject.FindGameObjectsWithTag("OWPlatform");
            for (int i = 0; i < owplatforms.Length; i++)
            {
                owplatforms[i].GetComponent<CompositeCollider2D>().enabled = false;
            }
            owplatforms2 = GameObject.FindGameObjectsWithTag("OWPlatform2");
            for (int i = 0; i < owplatforms2.Length; i++)
            {
                owplatforms2[i].SetActive(false);
            }

        }

        if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire2"))
        {
            if (canJump)
            {
                anim.SetBool("isJumping", true);
                Jump(Vector2.up);
                canJump = false;
                StartCoroutine(DisableGroundCheck());
            }
            else if ((coll.onRightWall) || (coll.onLeftWall) && !(coll.onGroundCenter))
            {
                anim.SetBool("isWalljumping", true);
                WallJump();
            }
        }

    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
        {
            return;
        }

        if (!canWallJump)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * walkSpeed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(dir.x * walkSpeed, rb.velocity.y);
            if(dir.x != 0)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }

    }

    private void WallJump()
    {
        rb.velocity = new Vector2(0, 0);
        if (coll.onRightWall)
        {
            Jump((Vector2.up * 1.2f + Vector2.left));
        }
        else if (coll.onLeftWall)
        {
            Jump((Vector2.up * 1.2f + Vector2.right));
        }
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.35f));

        canWallJump = false;

    }

    private void Jump(Vector2 dir) // Aplica vel para pulo apenas
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpSpeed;
    }

    private void LookingAt(Vector2 dir)
    {
        if (dir.y > 0) //lookingUp
        {
            lookingUp = true;
            lookingDown = false;
            maxFallSpeed = -17f;
        }
        else if (dir.y < 0) //lookingDown
        {
            lookingDown = true;
            lookingUp = false;
            maxFallSpeed = -30f;
        }
        else
        {
            lookingDown = false;
            lookingUp = false;
            maxFallSpeed = -17f;
        }

        if ((rb.velocity.x > 0 && !faceRight) || (rb.velocity.x < 0 && faceRight))
        {
            Flip();
        }
    }

    private void Flip() // Rotaciona o Player
    {
        transform.Rotate(0f, 180f, 0f);
        if (faceRight)
        {
            faceRight = false;
            anim.SetBool("lookingLeft", true);
        }
        else
        {
            faceRight = true;
            anim.SetBool("lookingLeft", false);
        }
    }

    private IEnumerator CoiyoteFunc() // Espera o coyote time para desabilitar o pulo
    {
        yield return new WaitForSeconds(coyoteTime);
        canJump = false;
    }

    private IEnumerator DisableMovement(float time) // Desativa o movimento do Player (dps do pulo de parede)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private void Shoot()
    {
        anim.SetBool("isAttacking", true);
        float xSide = 1f;
        if (!faceRight)
        {
            xSide = -1f;
        }
        if (coll.onRightWall && !(coll.onGroundCenter))
        {
            xSide = -1f;
        }
        else if (coll.onLeftWall && !(coll.onGroundCenter))
        {
            xSide = 1f;
        }

        var bulletGB = Instantiate(bullet, new Vector3(transform.position.x + (0.5f * xSide), transform.position.y, transform.position.z), transform.rotation);

        if (lookingUp)
        {
            bulletGB.GetComponent<Rigidbody2D>().velocity = new Vector2(15.32f * xSide, 12.856f);
        }
        else if (lookingDown)
        {
            bulletGB.GetComponent<Rigidbody2D>().velocity = new Vector2(15.32f * xSide, -12.856f);
        }
        else
        {
            bulletGB.GetComponent<Rigidbody2D>().velocity = new Vector2(20f * xSide, 0f);
        }


        StartCoroutine(ShootFunc());
    }

    private IEnumerator ShootFunc()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootTime);
        canShoot = true;
        anim.SetBool("isAttacking", false);
    }

    private void Fall(Vector2 dir)
    {
        // https://www.youtube.com/watch?v=7KiK0Aqtmzc

        if (rb.velocity.y < 0)
        {
            // WallSlide
            if (((dir.x > 0 && coll.onRightWall) || (dir.x < 0 && coll.onLeftWall)) &&
            !(coll.onGroundCenter))
            {
                anim.SetBool("isWall", true);
                anim.SetBool("isFalling", false);
                anim.SetBool("isWalljumping", false);
                anim.SetBool("isJumping", false);
                if (rb.velocity.y <= slideSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, slideSpeed);
                }
                canWallJump = true;
            }
            // Fall
            else
            {
                anim.SetBool("isWall", false);
                anim.SetBool("isFalling", true);
                anim.SetBool("isWalljumping", false);
                anim.SetBool("isJumping", false);
                rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
                if (rb.velocity.y <= maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);

                }
            }

        }
        // Short jump
        else if (rb.velocity.y > 0 && ((!Input.GetKey("space")) && (!Input.GetButton("Fire2"))))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private IEnumerator DisableGroundCheck() // Desativa o movimento do Player (dps do pulo de parede)
    {
        groundCheck = false;
        yield return new WaitForSeconds(0.1f);
        groundCheck = true;
    }

}
