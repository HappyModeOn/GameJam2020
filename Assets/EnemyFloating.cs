using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloating : MonoBehaviour
{

    public int hp = 10;
    public int currentHP = 10;


    public floatingPosition lenNumber;
    public Transform PlayerFloating;
    public float speed = 3;

    public List<NPCController> npcs;
    bool isBreak = false;
    public float delayAfterBreak = 3;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "HitBox")
        {
            if (other.transform.root.name != transform.root.name)
            {
                currentHP -= 1;
            }
               
        }
        if (currentHP <= 0)
        {
            for (int i = 0; i < npcs.Count; i++)
            {
                if (npcs[i].isDeath == false)
                {
                    npcs[i].currentHP = -1;
                }
            }
            isBreak = true;
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(Random.Range(-300, 300), Random.Range(500, 1500) , 0);
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddExplosionForce(500, transform.position, 3);
        }
    }

    public bool reachTarget = false;
    // Update is called once per frame
    void Update()
    {
        if (isBreak)
        {
            if (delayAfterBreak > 0)
            {
                delayAfterBreak -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
             
        }
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
