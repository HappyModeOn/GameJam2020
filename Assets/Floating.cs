using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public bool isPlayerFloating = false;
    public Floating topFloating;
    public Floating botFloating;
    public int hp = 10;
    public int currentHP = 10;


    public floatingPosition lenNumber;
    public Transform PlayerFloating;
    public float speed = 3;

    public List<NPCController> npcs;
    bool isBreak = false;
    public float delayAfterBreak = 3;
    // Start is called before the first frame update

    public bool reachTarget = false;

    public void CheckHP()
    {
        currentHP = npcs.Count;
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i].isDeath == true)
            {
                currentHP -= 1;
            }

        }
        if (currentHP <= 0)
        {
            isBreak = true;
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<Rigidbody>().isKinematic = false;
          //  GetComponent<Rigidbody>().AddForce(Random.Range(-700, 700), 700, 0);
          //  GetComponent<Rigidbody>().useGravity = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlayerFloating == false)// Hack
        {
            CheckHP();
        }
        

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
        if (isPlayerFloating)
        {
            if (topFloating.reachTarget)
            {
                reachTarget = true;
            }
            if (botFloating.reachTarget)
            {
                reachTarget = true;
            }
        }
        else
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
       
        
       
    }

    public enum floatingPosition
    {
        Top,
        Middle,
        Bottom
    }
}
