using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public int danceID = 0;
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
        if (danceID != -1)
        {
            anim.SetInteger("DanceID", danceID);
        }
        else
        {
            anim.SetInteger("DanceID", Random.Range(0,3));
        }
      
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
            Bounce();
        }

    }


    public void Bounce()
    {
        anim.SetTrigger("Impact");
        transform.parent = null;
        isDeath = true;
        rb.AddForce(knockbackPower);
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
        
        if (isDeath)
        {
            anim.SetBool("Death", true);
            if (transform.position.y < 2f)
            {
                anim.SetBool("OnWater", true);
                transform.position += Vector3.left * Time.deltaTime * speed/2;
            }
            return;
        }
        if (ef.reachTarget && isDeath == false)
        {
            if (isMelee)
            {
                if (ef.lenNumber == EnemyFloating.floatingPosition.Top)
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
                else if (ef.lenNumber == EnemyFloating.floatingPosition.Bottom)
                {
                    //transform.position += Vector3.forward * Time.deltaTime * speed;
                    anim.SetBool("Move", true);
                }
            }
        }
        
    }
}
