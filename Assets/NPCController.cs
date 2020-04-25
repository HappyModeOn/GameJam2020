﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ThrowerType
{
    Beer,
    Speaker
}
public class NPCController : MonoBehaviour
{
    int duty = 100;
    public MeshRenderer mr;
    public int danceID = 0;
    public bool isEnemy = false;
    public Animator anim;
    private Floating currentFloating;
    public float speed = 3;
    // Start is called before the first frame update
    public bool isMelee = false;
    public ThrowerType projectileType;
    public SimplePooling sp;

    public int hp = 3;
    public int currentHP = 3;

    private Rigidbody rb;


    void SetColorCup()
    {
        if (currentFloating.lenNumber == Floating.floatingPosition.Top)
        {
            mr.material.SetColor("_Color", Color.red);
        }
        else if (currentFloating.lenNumber == Floating.floatingPosition.Middle)
        {
            mr.material.SetColor("_Color", Color.yellow);
        }
        else if (currentFloating.lenNumber == Floating.floatingPosition.Bottom)
        {
            mr.material.SetColor("_Color", Color.green);
        }
    }
    void Start()
    {
        currentFloating = transform.root.GetComponent<Floating>();
        SetColorCup();
        if (projectileType == ThrowerType.Beer)
        {
            sp = GameObject.Find("BeerPool").GetComponent<SimplePooling>();
        }
       



        rb = gameObject.GetComponent<Rigidbody>();
        if (danceID != -1)
        {
            anim.SetInteger("DanceID", danceID);
        }
        else
        {
            anim.SetInteger("DanceID", Random.Range(0,3));
        }
      
    }


    void CheckDuty()
    {
        if (currentFloating.topFloating.reachTarget && currentFloating.botFloating.reachTarget)
        {
            duty = Random.Range(0, 100);
        }
        else
        {
            if (currentFloating.topFloating.reachTarget || currentFloating.botFloating.reachTarget)
            {
                if (currentFloating.topFloating.reachTarget)
                {
                    duty = 0;
                }
                else
                {
                    duty = 100;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ReviveZone")
        {
            if (!isEnemy)
            {
                return;
            }
            if (anim.GetBool("OnWater") == false)
            {
                return;
            }
            //hack Must Delay
            transform.parent = other.transform.root;
            isEnemy = false;
            currentHP = hp;
            isDeath = false;
            transform.position = other.transform.position;
            currentFloating = transform.root.GetComponent<Floating>();
            rb.isKinematic = false;
            if (currentFloating.npcs.Contains(this) == false)
            {
                currentFloating.npcs.Add(this);
            }

            CheckDuty();
            SetColorCup();
            ///hack careful for multiplayer
            GameObject.Find("Player").GetComponent<CharacterMovement>().currentLifeSave = false;
            if (danceID != -1)
            {
                anim.SetInteger("DanceID", danceID);
            }
            else
            {
                anim.SetInteger("DanceID", Random.Range(0, 3));
            }
            anim.SetBool("OnWater", false);
            anim.SetBool("Death", false);
            rb.velocity = Vector3.zero;
        }
        else if (other.name == "Saver")
        {
            if (other.transform.root.GetComponent<CharacterMovement>().currentLifeSave == false)
            {
                Debug.Log("Got Save");
                other.transform.root.gameObject.GetComponent<CharacterMovement>().currentLifeSave = true;
                transform.parent = other.transform.root;
                GetComponent<Rigidbody>().isKinematic = true;
                transform.localPosition = Vector3.zero;
            }
        }
        else if (other.name == "HitBox")
        {
            if (other.transform.root.name != transform.root.name)
            {
                if (other.GetComponent<HeavyObject>() != null)
                {
                    anim.SetTrigger("HeavyHurt");
                    currentHP -= 3;
                }
                else
                {
                    currentHP -= 1;
                    anim.SetTrigger("Hurt");
                }
            }

        }
        else if (other.GetComponent<ThrowingObject>() != null)
        {
            if (currentFloating.name != other.tag)
            {
                if (other.GetComponent<ThrowingObject>().damge > 1)
                {
                    anim.SetTrigger("HeavyHurt");
                    currentHP -= 3;
                }
                else
                {
                    currentHP -= 1;
                    anim.SetTrigger("Hurt");
                }
            }
            
        }
        else
        {
        }
        if (currentHP <= 0 && isDeath == false)
        {
            Bounce();
        }

    }


    public void Bounce()
    {
        currentFloating.npcs.Remove(this);
        gameObject.layer = LayerMask.NameToLayer("Drowner");
        anim.SetTrigger("Impact");
        transform.parent = null;
        isDeath = true;
        rb.AddForce(new Vector3(Random.Range(-500, 500), Random.Range(300, 700), Random.Range(-300, 300)));
        currentFloating.npcs.Remove(this);
        anim.SetBool("Death", true);
    }
    // Update is called once per frame

    public Vector3 knockbackPower;
    public bool isDeath = false;

    public float attackCD = 5;
    private float currentAttackCD = 5;
    public GameObject hitBox;
    void Update()
    {
        //rb.MovePosition(Vector3.forward * speed * Time.deltaTime);
        if (anim.GetBool("OnWater"))
        {
            if (transform.parent == null)
            {
                transform.position += Vector3.left * Time.deltaTime * speed / 5;
            }
           
        }
        if (isDeath)
        {
            
            if (transform.position.y < 1.5f)
            {
                //rb.velocity = Vector3.zero;
                anim.SetBool("OnWater", true);
            }
           
        
            return;
        }

        if (currentFloating.reachTarget)
        {
            if (isMelee)
            {
                if (currentFloating.lenNumber == Floating.floatingPosition.Top)
                {
                    if (transform.localPosition.z > -0.45f)
                    {
                        rb.AddForce(Vector3.back * speed);
                        //transform.position += Vector3.back * Time.deltaTime * speed;
                        anim.SetBool("Move", true);
                    }
                    else
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.45f);
                        rb.velocity = Vector3.zero;
                        anim.SetInteger("DanceID", -1);
                        anim.SetBool("Move", false);
                        if (currentAttackCD > 0)
                        {
                            currentAttackCD -= Time.deltaTime;
                        }
                        else
                        {
                            currentAttackCD = attackCD;
                            anim.SetTrigger("Attack");
                            hitBox.SetActive(true);
                        }
                    }
                }
                else if (currentFloating.lenNumber == Floating.floatingPosition.Middle)
                {

                    if (duty < 50)
                    {
                        if (transform.localPosition.z > -0.4f)
                        {
                            rb.AddForce(Vector3.back * speed);
                            anim.SetBool("Move", true);

                        }
                        else
                        {

                            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.4f);
                            rb.velocity = Vector3.zero;
                            anim.SetInteger("DanceID", -1);
                            anim.SetBool("Move", false);
                            if (currentAttackCD > 0)
                            {
                                currentAttackCD -= Time.deltaTime;
                            }
                            else
                            {
                                CheckDuty();
                                currentAttackCD = attackCD;
                                anim.SetTrigger("Attack");
                                hitBox.SetActive(true);
                            }
                        }

                    }
                    else
                    {
                        if (transform.localPosition.z < 0.4f)
                        {
                            rb.AddForce(Vector3.forward * speed);
                            anim.SetBool("Move", true);
                        }
                        else
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.4f);
                            rb.velocity = Vector3.zero;
                            anim.SetInteger("DanceID", -1);
                            anim.SetBool("Move", false);
                            if (currentAttackCD > 0)
                            {
                                currentAttackCD -= Time.deltaTime;
                            }
                            else
                            {
                                CheckDuty();
                                currentAttackCD = attackCD;
                                anim.SetTrigger("Attack");
                                hitBox.SetActive(true);
                            }
                        }
                    }
                }
                else if (currentFloating.lenNumber == Floating.floatingPosition.Bottom)
                {
                    if (transform.localPosition.z < 0.45f)
                    {
                        rb.AddForce(Vector3.forward * speed);
                        anim.SetBool("Move", true);
                    }
                    else
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.45f);
                        rb.velocity = Vector3.zero;
                        anim.SetInteger("DanceID", -1);
                        anim.SetBool("Move", false);
                        if (currentAttackCD > 0)
                        {
                            currentAttackCD -= Time.deltaTime;
                        }
                        else
                        {
                            currentAttackCD = attackCD;
                            anim.SetTrigger("Attack");
                            hitBox.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                if (currentAttackCD > 0)
                {
                    currentAttackCD -= Time.deltaTime;
                }
                else
                {
                    currentAttackCD = attackCD;
                    anim.SetTrigger("Attack");
                    GameObject trb = sp.Show(transform.position, currentFloating.name);
                    if (currentFloating.lenNumber == Floating.floatingPosition.Top)
                    {
                        trb.GetComponent<ThrowingObject>().directionZ = -1;
                    }
                    else if (currentFloating.lenNumber == Floating.floatingPosition.Middle)
                    {
                    }
                    else if (currentFloating.lenNumber == Floating.floatingPosition.Bottom)
                    {
                        trb.GetComponent<ThrowingObject>().directionZ = 1;
                    }
                }
             

            }
        }
        
        
        
    }
}
