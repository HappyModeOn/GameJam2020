using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloating : MonoBehaviour
{
    public Transform PlayerFloating;
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = transform.position.x - PlayerFloating.position.x;
        if (Mathf.Abs(dist) < 0.1f)
        {
            transform.position = new Vector3(PlayerFloating.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
       
    }
}
