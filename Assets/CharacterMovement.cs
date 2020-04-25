using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Animator anim;
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    public GameObject hitbox;

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "HitBox")
        {
            if (other.transform.root.name != transform.root.name)
            {

                anim.SetTrigger("Hurt");
                if (other.transform.position.z > transform.position.z)
                {
                    moveDirection = new Vector3(0, 0.1f, -0.5f);
                }
                else
                {
                    moveDirection = new Vector3(0, 0.1f, 0.5f);

                }
                currentHurtTime = hurtTime;
            }
        }
        else if (other.GetComponent<ThrowingObject>() != null)
        {
            if (other.GetComponent<ThrowingObject>().damge > 1)
            {
                anim.SetTrigger("HeavyHurt");
                if (other.transform.position.z > transform.position.z)
                {
                    moveDirection = new Vector3(0, 0.2f, -0.5f);
                }
                else
                {
                    moveDirection = new Vector3(0, 0.2f, 0.5f);
                }
            }
        }
    }
    public MeshRenderer meshRenderer;
    public float hurtTime = 3;
    private float currentHurtTime = 0;
    public bool currentLifeSave = false;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim.GetComponent<Animator>();

        meshRenderer.material.SetColor("_Color", Color.yellow);

    }

    public GameObject saverTrigger;
    void Update()
    {
        if (transform.position.y < 2f)
        {
            anim.SetBool("OnWater", true);
            saverTrigger.SetActive(true);
        }
        else
        {
            anim.SetBool("OnWater", false);
            saverTrigger.SetActive(false);
        }
        if (currentHurtTime > 0)
        {
            currentHurtTime -= Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
            return;
        }

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            if (moveDirection != Vector3.zero)
            {
                anim.SetBool("Move", true);
                if (moveDirection.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (moveDirection.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                if (transform.position.y < 2f)
                {
                    
                    moveDirection.x = -0.15f;
                }
                  
                anim.SetBool("Move", false);
                anim.SetInteger("DanceID", Random.Range(0, 3));
            }
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

            if (transform.position.y < 2f)
            {
                if (moveDirection.x > 0)
                {
                  
                    if (currentLifeSave)
                    {
                        moveDirection.x = moveDirection.x / 4;
                    }
                    else
                    {
                        moveDirection.x = moveDirection.x / 2;
                    }
                }
            }
            if (Input.GetButton("Fire1"))
            {
                if (hitbox.activeInHierarchy == false)
                {
                    anim.SetTrigger("Attack");
                    hitbox.SetActive(true);
                }
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
