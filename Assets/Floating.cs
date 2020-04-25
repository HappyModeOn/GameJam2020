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


    public int visualID = 0;
    public GameObject[] visualBoat;
    public AudioClip[] boatSong;
    public AudioClip[] bossSong;

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
                questPanel.SetActive(false);
            }
            currentWave += 1;
        }
    }


    IEnumerator DelayPlayerBGM()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(7);

        if (PlayerFloating.GetComponent<AudioSource>() != null)
        {
            PlayerFloating.GetComponent<AudioSource>().Play();
        }
    }
   
public GameObject questPanel;
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
            questPanel.SetActive(true);
            return;
        }



        if (visualBoat.Length > 0)
        {
            for (int i = 0; i < visualBoat.Length; i++)
            {
                visualBoat[i].SetActive(false);
            }
        }

        //Gen NPC then start Move Right Again
        isBreak = false;
        reachTarget = false;
        int numberOfNPC = 2;
        numberOfNPC += currentWave % 3;
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
        if (currentWave % 5 == 0 && lenNumber == floatingPosition.Bottom)
        {
            GameObject newNPC = npcG.AddUniqueNPC(transform.position, BossCount);
            //GameObject newNPC = npcG.AddNPC(transform.position, 1);
            newNPC.transform.localScale = Vector3.one;
            newNPC.transform.parent = transform;
            npcs.Add(newNPC.GetComponent<NPCController>());


           // visualID = Random.Range(0, visualBoat.Length);
            GetComponent<AudioSource>().clip = boatSong[boatSongID];
            boatSongID++;
            if (boatSongID > 2)
            {
                boatSongID = 0;
            }
            GetComponent<AudioSource>().clip = bossSong[BossCount];
            GetComponent<AudioSource>().Play();
            BossCount++;

            if (BossCount > 2)
            {
                BossCount = 0;
            }
        }
        else
        {
            visualID = Random.Range(0, boatSong.Length);
           // visualBoat[visualID].SetActive(true);
            GetComponent<AudioSource>().clip = boatSong[visualID];
            
        }

    }

    public int boatSongID = 0;
    public int BossCount = 0;

    private int currentWave = 1;
    public void Start()
    {
        StartCoroutine(DelayPlayerBGM());
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
                if (transform.position.x > -50)
                {
                    transform.Translate(Vector3.left * speed*2 * Time.deltaTime);
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

                    if (Mathf.Abs(dist) < 5f)
                    {
                        if (GetComponent<AudioSource>().isPlaying == false)
                        {
                            GetComponent<AudioSource>().Play();
                        }
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
