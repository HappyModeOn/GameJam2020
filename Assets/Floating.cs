using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public bool isBreak = false;
    public bool reachTarget = false;




    public NPCGenerator npcG;
    public bool isPlayerFloating = false;
    public Floating topFloating;
    public Floating botFloating;
    private int currentHP = 10;


    public floatingPosition lenNumber;
    public Transform PlayerFloating;
    public float speed = 3;

    public List<NPCController> npcs;
   

    

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
            GetComponent<AudioSource>().Stop();
            isBreak = true;
            reachTarget = false;

            if (currentWave == 1)
            {
                botFloating.gameObject.SetActive(true);
            }
            currentWave += 1;
        }
    }
    public AudioClip[] ac;

    void PrepareNewFloating()
    {
        if (isPlayerFloating)
        {

            GameObject newNPC = npcG.AddMeleeNPC(transform.position);
            //GameObject newNPC = npcG.AddNPC(transform.position, 1);
            newNPC.transform.localScale = Vector3.one;
            newNPC.transform.parent = transform;
            newNPC.GetComponent<NPCController>().isEnemy = false;
            npcs.Add(newNPC.GetComponent<NPCController>());

            GameObject newNPC1 = npcG.AddThrowerNPC(transform.position, 1);
            //GameObject newNPC = npcG.AddNPC(transform.position, 1);
            newNPC1.transform.localScale = Vector3.one;
            newNPC1.transform.parent = transform;
            newNPC1.GetComponent<NPCController>().isEnemy = false;
            npcs.Add(newNPC1.GetComponent<NPCController>());

            return;
        }
        if (ac.Length > 0)
        {
            GetComponent<AudioSource>().clip = ac[Random.Range(0, ac.Length)];
        }
      
        GetComponent<AudioSource>().Play();
        //Gen NPC then start Move Right Again
        isBreak = false;
        reachTarget = false;
        int numberOfNPC = 3;
        numberOfNPC += currentWave % 5;
        for (int i = 0; i < numberOfNPC; i++)
        {
            if (i % 3 == 0)
            {
                GameObject newNPC = npcG.AddMeleeNPC(transform.position);
                //GameObject newNPC = npcG.AddNPC(transform.position, 1);
                newNPC.transform.localScale = Vector3.one;
                newNPC.transform.parent = transform;
                npcs.Add(newNPC.GetComponent<NPCController>());
            }
            else
            {
                GameObject newNPC = npcG.AddThrowerNPC(transform.position);
                //GameObject newNPC = npcG.AddNPC(transform.position, 1);
                newNPC.transform.localScale = Vector3.one;
                newNPC.transform.parent = transform;
                npcs.Add(newNPC.GetComponent<NPCController>());
            }
           
        }
        Debug.Log(currentWave + "/ mod 5 >>" + (currentWave %5) + "/ mod 3 >>" + (currentWave % 3));
        if (currentWave % 3 == 0)
        {
            GameObject newNPC = npcG.AddUniqueNPC(transform.position);
            //GameObject newNPC = npcG.AddNPC(transform.position, 1);
            newNPC.transform.localScale = Vector3.one;
            newNPC.transform.parent = transform;
            npcs.Add(newNPC.GetComponent<NPCController>());
        }
    }

    private int currentWave = 1;
    public void Start()
    {
        PrepareNewFloating();
    }
   public GameObject gameOverPanel;
    void Update()
    {



        if (isBreak)
        {
            if (isPlayerFloating)
            {
                GetComponent<AudioSource>().Stop();
                gameOverPanel.SetActive(false);

            }
            else
            {
                reachTarget = false;
                if (transform.position.x > -30)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                else
                {
                    PrepareNewFloating();
                }
            }
            
        }
        else
        {
            CheckHP();
            if (isPlayerFloating)
            {

            }
            else
            {
                
                if (reachTarget == false)
                {
                    float dist = transform.position.x - PlayerFloating.position.x;
                    if (Mathf.Abs(dist) < 0.1f)
                    {
                        // _startPosition = transform.position;

                        reachTarget = true;
                        transform.position = new Vector3(PlayerFloating.position.x, transform.position.y, transform.position.z);

                        //transform.position = _startPosition + new Vector3(Mathf.Sin(Time.time), transform.position.y, transform.position.z);
                    }
                    else
                    {
                        transform.Translate(Vector3.right * speed * Time.deltaTime);
                    }
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
