using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum DockingDirection
{
    None,
    Top,
    Both,
    Bot
}
public enum ThrowerType
{
    Beer,
    Plate,
    Flipflop,
    Mic,
    Redcup,
    Speaker
}
public class NPCController : MonoBehaviour
{
    public AudioClip waterSplashSFX;
    public AudioClip bwawawawSFX;
    public AudioClip savingSFX;
    public AudioClip savedSFX;
    public DockingDirection duty = DockingDirection.None;
    public MeshRenderer mr;
    public int danceID = 0;
    public bool isEnemy = false;
    public Animator anim;
    public Floating currentFloating;
    public float speed = 3;
    // Start is called before the first frame update
    public bool isMelee = false;
    public ThrowerType projectileType;
    public SimplePooling sp;

    public int hp = 3;
    private int currentHP = 3;

    private Rigidbody rb;

    public AudioClip[] hurtSFX;
    void SetColorCup()
    {
        if (currentFloating.lenNumber == Floating.floatingPosition.Top)
        {
            mr.material.SetColor("_Color", Color.red);
        }
        else
        {
            mr.material.SetColor("_Color", Color.blue);
        }
    }
    void Start()
    {
        currentHP = hp;
        currentFloating = transform.root.GetComponent<Floating>();
        SetColorCup();
        if (projectileType == ThrowerType.Beer)
        {
            sp = GameObject.Find("BeerPool").GetComponent<SimplePooling>();
        }
        else if (projectileType == ThrowerType.Flipflop)
        {
            sp = GameObject.Find("FlipflopPool").GetComponent<SimplePooling>();
        }
        if (projectileType == ThrowerType.Mic)
        {
            sp = GameObject.Find("MicPool").GetComponent<SimplePooling>();
        }
        if (projectileType == ThrowerType.Plate)
        {
            sp = GameObject.Find("PlatePool").GetComponent<SimplePooling>();
        }
        if (projectileType == ThrowerType.Redcup)
        {
            sp = GameObject.Find("RedcupPool").GetComponent<SimplePooling>();
        }
        if (projectileType == ThrowerType.Speaker)
        {
            sp = GameObject.Find("SpeakerPool").GetComponent<SimplePooling>();
        }




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


    void CheckDuty()
    {
        if (currentFloating.topFloating.reachTarget && currentFloating.botFloating.reachTarget)
        {
            int ran = Random.Range(0, 100);
            if (ran > 50)
            {

                duty = DockingDirection.Top;
            }
            else
            {
                duty = DockingDirection.Bot;
            }
        
        }
        else if (currentFloating.topFloating.reachTarget || currentFloating.botFloating.reachTarget)
        {
            if (currentFloating.topFloating.reachTarget)
            {
                duty = DockingDirection.Top;
            }
            else
            {
                duty = DockingDirection.Bot;
            }
        }
        else
        {
            duty = DockingDirection.None;
        }
            
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ReviveZone")
        {
            if (anim.GetBool("Rescue") == false)
            {
                return;
            }
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Impact");
            anim.ResetTrigger("Hurt");
            anim.ResetTrigger("HeavyHurt");
            anim.SetBool("Move", false);
            GetComponent<AudioSource>().PlayOneShot(savedSFX);
            //hack Must Delay
            transform.parent = other.transform.root;
            isEnemy = false;
            currentHP = hp;
            isDeath = false;
            currentFloating = transform.root.GetComponent<Floating>();
            
            if (currentFloating.npcs.Contains(this) == false)
            {
                currentFloating.npcs.Add(this);
            }
            GameObject.Find("Player").GetComponent<CharacterMovement>().npcOnHand = null;
            CheckDuty();
            SetColorCup();
            ///hack careful for multiplayer
            if (danceID != -1)
            {
                anim.SetInteger("DanceID", danceID);
            }
            else
            {
                anim.SetInteger("DanceID", Random.Range(0, 3));
            }
            gameObject.layer = LayerMask.NameToLayer("NPC");
            anim.SetBool("OnWater", false);
            anim.SetBool("Death", false);
            rb.velocity = Vector3.zero;
            rb.isKinematic = false;
            anim.SetBool("Rescue", false);
        }
        else if (other.name == "Saver")
        {
            if (other.transform.root.GetComponent<CharacterMovement>().npcOnHand == null)
            {
                GetComponent<AudioSource>().PlayOneShot(savingSFX);
                anim.SetBool("Rescue", true);
                Debug.Log("Got Save");
                other.transform.root.gameObject.GetComponent<CharacterMovement>().npcOnHand = this;
                transform.parent = other.transform.root;
                GetComponent<Rigidbody>().isKinematic = true;
                transform.localPosition = Vector3.zero;
            }
        }
        else if (other.name == "HitBox")
        {
            if (currentFloating.isPlayerFloating && other.transform.root.name == "Player")
            {
                return;
            }
            if (other.transform.root.name != transform.root.name)
            {
                GetComponent<AudioSource>().clip = hurtSFX[Random.Range(0, hurtSFX.Length)];
                GetComponent<AudioSource>().Play();
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
        else if (other.GetComponent<ThrowingObject>() != null)
        {
            if (currentFloating.name != other.tag)
            {
                GetComponent<AudioSource>().clip = hurtSFX[Random.Range(0, hurtSFX.Length)];
                GetComponent<AudioSource>().Play();
                if (other.GetComponent<ThrowingObject>().damge > 1)
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
        else
        {
        }
        if (currentHP <= 0 && isDeath == false)
        {
            Bounce();
        }

    }

    private void Death()
    {
        isDeath = true;
        currentFloating.npcs.Remove(this);
        gameObject.layer = LayerMask.NameToLayer("Drowner");
        anim.SetBool("Death", true);
    }
    public void Bounce()
    {
        GetComponent<AudioSource>().PlayOneShot(bwawawawSFX);
        Death();
        anim.SetTrigger("Impact");
        transform.parent = null;
       
        rb.AddForce(new Vector3(Random.Range(300, 500), Random.Range(150, 350), Random.Range(-50, 50)));
     
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
        if (anim.GetBool("OnWater"))
        {

            if (transform.parent == null)
            {
                transform.position += Vector3.left * Time.deltaTime * speed / 2;
            }
            else
            {
                if (isDeath == false)
                {
                    Death();
                }
            }

        }
        if (isDeath)
        {

            if (transform.position.y < 1.5f)
            {
                //rb.velocity = Vector3.zero;
                anim.SetBool("OnWater", true);
                GetComponent<AudioSource>().clip = waterSplashSFX;
                GetComponent<AudioSource>().Play();


            }
            if (transform.position.x < -15f)
            {
                Destroy(gameObject);
            }


            return;
        }

        if (currentFloating.isPlayerFloating)
        {
            if (currentFloating.topFloating.reachTarget == false && currentFloating.botFloating.reachTarget == false)
            {
                CheckDuty();
                return;
            }
            if (isMelee)
            {
                if (duty == DockingDirection.Bot)
                {
                    if (transform.localPosition.z > -0.4f)
                    {
                        rb.AddForce(Vector3.back * speed);
                        anim.SetBool("Move", true);

                    }
                    else
                    {

                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.4f);
                        rb.velocity = Vector3.zero;
                        anim.SetInteger("DanceID", -1);
                        anim.SetBool("Move", false);
                        if (currentAttackCD > 0)
                        {
                            currentAttackCD -= Time.deltaTime;
                        }
                        else
                        {
                            CheckDuty();
                            currentAttackCD = attackCD;
                            anim.SetTrigger("Attack");
                            hitBox.SetActive(true);
                        }
                    }

                }
                else if (duty == DockingDirection.Top)
                {
                    if (transform.localPosition.z < 0.4f)
                    {
                        rb.AddForce(Vector3.forward * speed);
                        anim.SetBool("Move", true);
                    }
                    else
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.4f);
                        rb.velocity = Vector3.zero;
                        anim.SetInteger("DanceID", -1);
                        anim.SetBool("Move", false);
                        if (currentAttackCD > 0)
                        {
                            currentAttackCD -= Time.deltaTime;
                        }
                        else
                        {
                            CheckDuty();
                            currentAttackCD = attackCD;
                            anim.SetTrigger("Attack");
                            hitBox.SetActive(true);
                        }
                    }
                }
                
            }
            else
            {
                if (currentAttackCD > 0)
                {
                    currentAttackCD -= Time.deltaTime;
                }
                else
                {
                    CheckDuty();
                    currentAttackCD = attackCD;
                    anim.SetTrigger("Attack");
                   

                    if (duty == DockingDirection.Top)
                    {
                        sp.Show(transform.position, currentFloating.name, 1);
                    }
                    else
                    {
                        sp.Show(transform.position, currentFloating.name, -1);
                    }
                    
                }
            }
        }
        else
        {
            if (currentFloating.reachTarget)
            {
                if (isMelee)
                {
                    if (currentFloating.lenNumber == Floating.floatingPosition.Top)
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
                    else if (currentFloating.lenNumber == Floating.floatingPosition.Bottom)
                    {
                        if (transform.localPosition.z < 0.45f)
                        {
                            rb.AddForce(Vector3.forward * speed);
                            anim.SetBool("Move", true);
                        }
                        else
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.45f);
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
                }
                else
                {
                    if (currentAttackCD > 0)
                    {
                        currentAttackCD -= Time.deltaTime;
                    }
                    else
                    {
                        CheckDuty();
                        currentAttackCD = attackCD;
                        anim.SetTrigger("Attack");
                       
                        if (currentFloating.lenNumber == Floating.floatingPosition.Top)
                        {
                            sp.Show(transform.position, currentFloating.name, -1);
                        }
                        else if (currentFloating.lenNumber == Floating.floatingPosition.Bottom)
                        {
                            sp.Show(transform.position, currentFloating.name , 1);
                        }
                    }


                }
            }
        }

        
        
        
        
    }
}
