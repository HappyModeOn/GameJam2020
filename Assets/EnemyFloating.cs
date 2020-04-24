using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloating : MonoBehaviour
{
    public floatingPosition lenNumber;
    public Transform PlayerFloating;
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool reachTarget = false;
    // Update is called once per frame
    void Update()
    {
        if (reachTarget == false)
        {
            float dist = transform.position.x - PlayerFloating.position.x;
            if (Mathf.Abs(dist) < 0.1f)
            {
                reachTarget = true;
                transform.position = new Vector3(PlayerFloating.position.x, transform.position.y, transform.position.z);
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
        
       
    }

    public enum floatingPosition
    {
        Top,
        Bottom
    }
}
