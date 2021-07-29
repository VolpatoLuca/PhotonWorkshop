using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : Photon.MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool isGrounded;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float fireRate = 2;
    [SerializeField] private GameObject cam;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private Transform rightSpawnPos;
    [SerializeField] private Transform leftSpawnPos;

    private float horizontalInput;
    private float fireTimer;
    private bool isFiring;
    private bool isAirborne;
    private bool canDoubleJump;
    private bool jumpInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (photonView.isMine)
        {
            cam.SetActive(true);
            usernameText.text = PhotonNetwork.playerName;
        }
        else
        {
            usernameText.text = photonView.owner.NickName;
            usernameText.color = Color.blue;
        }
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            isGrounded = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f, 1<<LayerMask.NameToLayer("Ground")).Length != 0;
            if (isGrounded)
                canDoubleJump = true;
            else if(!isFiring && !isAirborne)
            {
                isAirborne = true;
                anim.SetTrigger("StartJump");
            }

            if (isGrounded && rb.velocity.y < 0)
            {
                anim.SetTrigger("EndJump");
            }

            GetInputs();

            Jump();
            Fire();
        }
    }

    private void Jump()
    {
        if (jumpInput && (isGrounded || canDoubleJump))
        { 
            if (!isGrounded)
                canDoubleJump = false;
            Vector2 vel = Vector2.zero;
            vel.x = rb.velocity.x;
            vel.y = jumpForce;
            rb.velocity = vel;
            anim.ResetTrigger("EndJump");
            anim.SetTrigger("StartJump");
        }
    }

    private void Fire()
    {
        anim.SetBool("IsShooting", isFiring);
        if (isFiring)
        {
            isAirborne = false;
            fireTimer += Time.deltaTime;
            if (fireTimer >= 1 / fireRate)
            {
               
                fireTimer = 0;
                Vector3 spawnPos = sr.flipX ? leftSpawnPos.position : rightSpawnPos.position;
                GameObject o = PhotonNetwork.Instantiate(projectilePrefab.name, spawnPos, Quaternion.identity, 0);
                if (sr.flipX)
                    o.GetComponent<PhotonView>().RPC("FlipX", PhotonTargets.AllBuffered);
            }
        }
    }

    private void GetInputs()
    {
        jumpInput = Input.GetButtonDown("Jump");


        horizontalInput = Input.GetAxisRaw("Horizontal") * moveSpeed;
        isFiring = Input.GetButton("Fire1");


        if (horizontalInput > 0 && sr.flipX)
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
        else if (horizontalInput < 0 && !sr.flipX)
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            rb.velocity = new Vector2(horizontalInput, rb.velocity.y);
            anim.SetBool("IsRunning", horizontalInput != 0);
        }
    }

    [PunRPC]            //Remote Procedure Call 
    private void FlipTrue()
    {
        if (!sr)
            sr = GetComponent<SpriteRenderer>();
        sr.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }
}
