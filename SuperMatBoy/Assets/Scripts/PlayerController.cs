﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LevelGeneration levelGen;
    private Renderer rend;
    private Rigidbody2D rigid;
    public Transform startingPosition;
    public float maxSpeed = 10;
    public float acceleration = 35;
    public float jumpSpeed = 8;
    public float jumpDuration;
    public bool wallHitDJOverride = true;

    int jump = 2;
    bool wallJump = false;
    float jmpDuration;

    bool jumpKewDown = false;
    bool canVariableJump = false;
    bool onTheGround = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody2D>();
        transform.position = startingPosition.position + new Vector3(-3, 0, 0);
    }


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal < -0.1f)
        {
            if (rigid.velocity.x > -this.maxSpeed)
            {
                rigid.AddForce(new Vector2(-this.acceleration, 0f));
            }
            else
            {
                rigid.velocity = new Vector2(-this.maxSpeed, rigid.velocity.y);
            }
        }
        else if (horizontal > 0.1f)
        {
            if (rigid.velocity.x < this.maxSpeed)
            {
                rigid.AddForce(new Vector2(this.acceleration, 0f));
            }
            else
            {
                rigid.velocity = new Vector2(this.maxSpeed, rigid.velocity.y);
            }
        }



        float vertical = Input.GetAxis("Vertical");

        if (onTheGround)
        {
            jump = 2;
        }

        if (Input.GetButtonDown("Jump") && jump > 0)
        {
            if (!jumpKewDown)
            {
                jumpKewDown = true;
                if (onTheGround || wallHitDJOverride)
                {
                    bool wallHit = false;
                    int wallHitDirection = 0;
                    onTheGround = false;

                    bool leftWallHit = isWallOnLeft();
                    bool rightWallHit = isWallOnRight();

                    if (horizontal != 0)
                    {
                        if (leftWallHit)
                        {
                            wallHit = true;
                            wallHitDirection = 1;
                            jump = 2;
                        }
                        else if (rightWallHit)
                        {
                            wallHit = true;
                            wallHitDirection = -1;
                            jump = 2;
                        }
                    }
                    if (!wallHit)
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, this.jumpSpeed);

                        jmpDuration = 0.0f;

                        canVariableJump = true;

                        jump--;
                    }
                    else
                    {
                        rigid.velocity = new Vector2(this.jumpSpeed * wallHitDirection, this.jumpSpeed * 1.2f);

                        jmpDuration = 0.0f;

                        canVariableJump = true;

                        jump--;
                    }

                }
                else if (canVariableJump)
                {
                    jmpDuration += Time.deltaTime;

                    if (jmpDuration < this.jumpDuration / 1000 && jump > 0)
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, this.jumpSpeed);
                        jump--;
                    }
                }
            }
        }
        else
        {
            jumpKewDown = false;
            canVariableJump = false;
        }
    }
    private bool isWallOnLeft()
    {
        if (wallJump)
        {
            bool retVal = false;
            float lenghtToSearch = 0.1f;
            float colliderTreshold = 0.01f;

            Vector2 linestart = new Vector2(this.transform.position.x - this.rend.bounds.extents.x - colliderTreshold, this.transform.position.y);

            Vector2 vectorToSearch = new Vector2(linestart.x - lenghtToSearch, this.transform.position.y);

            RaycastHit2D hitLeft = Physics2D.Linecast(linestart, vectorToSearch);

            retVal = hitLeft;

            return retVal;
        }
        else
        {
            return false;
        }
    }

    private bool isWallOnRight()
    {
        if (wallJump)
        {
            bool retVal = false;
            float lenghtToSearch = 0.1f;
            float colliderTreshold = 0.01f;

            Vector2 linestart = new Vector2(this.transform.position.x + this.rend.bounds.extents.x + colliderTreshold, this.transform.position.y);

            Vector2 vectorToSearch = new Vector2(linestart.x + lenghtToSearch, this.transform.position.y);

            RaycastHit2D hitRight = Physics2D.Linecast(linestart, vectorToSearch);

            retVal = hitRight;

            return retVal;
        }
        else
        {
            return false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            wallJump = true;
            onTheGround = true;
        }
        if (collision.gameObject.layer == 9)
        {
            wallJump = false;
        }
        if (collision.gameObject.layer == 10)
        {
            wallJump = true;
            jump = 2;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("pushingTrapBot"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 20);
        }
        if (col.CompareTag("pushingTrapLeft"))
        {
            rigid.velocity = new Vector2(20, rigid.velocity.y);
        }
        if (col.CompareTag("pushingTrapRight"))
        {
            rigid.velocity = new Vector2(-20, rigid.velocity.y);
        }

    }
}
