using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyFloating ef;
    public float speed = 3;
    // Start is called before the first frame update
    public bool isMelee = false;

    private Rigidbody rb;
    void Start()
    {
        ef = transform.root.GetComponent<EnemyFloating>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.MovePosition(Vector3.forward * speed * Time.deltaTime);
        
        if (ef.reachTarget)
        {
            if (isMelee)
            {
                if (ef.lenNumber == EnemyFloating.floatingPosition.Top)
                {
                    transform.position += Vector3.back * Time.deltaTime * speed;


                }
                else if (ef.lenNumber == EnemyFloating.floatingPosition.Bottom)
                {
                    transform.position += Vector3.forward * Time.deltaTime * speed;
                }
            }
        }
        
    }
}
