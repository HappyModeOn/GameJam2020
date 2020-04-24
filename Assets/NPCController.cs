using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool isEnemy = false;
    public Animator anim;
    private EnemyFloating ef;
    public float speed = 3;
    // Start is called before the first frame update
    public bool isMelee = false;

    public int hp = 3;
    public int currentHP = 3;

    private Rigidbody rb;
    void Start()
    {
        ef = transform.root.GetComponent<EnemyFloating>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ReviveZone")
        {
            isEnemy = false;
            currentHP = hp;
            isDeath = false;
        }

        if (other.name == "HitBox")
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
        if (currentHP <= 0 && isDeath == false)
        {
            transform.parent = null;
            rb.AddForce(knockbackPower);
            anim.SetTrigger("Impact");
            isDeath = true;
        }

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
        if (transform.position.y < 2 )
        {
            anim.SetBool("OnWater", true);
            transform.position += Vector3.left * Time.deltaTime * speed;
           // transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        if (isDeath)
        {
            anim.SetBool("Death", true);
            return;
        }
        if (ef.reachTarget)
        {
            if (isMelee)
            {
                if (ef.lenNumber == EnemyFloating.floatingPosition.Top)
                {
                    if (transform.position.z > 2)
                    {
                        transform.position += Vector3.back * Time.deltaTime * speed;
                        anim.SetBool("Move", true);
                    }
                    else
                    {
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
                else if (ef.lenNumber == EnemyFloating.floatingPosition.Bottom)
                {
                    transform.position += Vector3.forward * Time.deltaTime * speed;
                    anim.SetBool("Move", true);
                }
            }
        }
        
    }
}
