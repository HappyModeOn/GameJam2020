using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
        if (other.name == "HitBox")
        {
            currentHP -= 1;
        }
       
    }
    // Update is called once per frame

    public Vector3 knockbackPower;
    public bool isDeath = false;
    void Update()
    {
        //rb.MovePosition(Vector3.forward * speed * Time.deltaTime);
        if (isDeath)
        {
            return;
        }
        if (currentHP <= 0)
        {
            transform.parent = null;
            rb.AddForce(knockbackPower);
            isDeath = true;
        }
        if (ef.reachTarget)
        {
            if (isMelee)
            {
                if (ef.lenNumber == EnemyFloating.floatingPosition.Top)
                {
                    transform.position += Vector3.back * Time.deltaTime * (speed - Random.Range(0,speed-1));


                }
                else if (ef.lenNumber == EnemyFloating.floatingPosition.Bottom)
                {
                    transform.position += Vector3.forward * Time.deltaTime * speed;
                }
            }
        }
        
    }
}
