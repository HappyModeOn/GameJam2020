using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{
    public GameObject[] npcBoss;
    public GameObject[] npcMelee;
    public GameObject[] npcProjectile;

    float rangeOfRandom = 1f;

    public GameObject AddMeleeNPC(Vector3 pos, int npcID = -1)
    {
        if (npcID == -1)
        {
            npcID = Random.Range(0, npcMelee.Length);
        }
        pos += new Vector3(Random.Range(-rangeOfRandom, rangeOfRandom), 1, Random.Range(-rangeOfRandom, rangeOfRandom));
        GameObject clone = Instantiate(npcMelee[npcID], pos, Quaternion.identity) as GameObject;


        return clone;

    }
    public GameObject AddThrowerNPC(Vector3 pos, int npcID = -1)
    {
        if (npcID == -1)
        {
            npcID = Random.Range(0, npcProjectile.Length);
        }
        pos += new Vector3(Random.Range(-rangeOfRandom, rangeOfRandom), 1, Random.Range(-rangeOfRandom, rangeOfRandom));
        GameObject clone = Instantiate(npcProjectile[npcID], pos, Quaternion.identity) as GameObject;


        return clone;

    }
    public GameObject AddUniqueNPC(Vector3 pos, int npcID = -1, bool randomPos = false)
    {
        if (npcID == -1)
        {
            npcID = Random.Range(0, npcBoss.Length);
        }
        if (randomPos)
        {
            pos += new Vector3(Random.Range(-rangeOfRandom, rangeOfRandom), 1, Random.Range(-rangeOfRandom, rangeOfRandom));
        }
        
        GameObject clone = Instantiate(npcBoss[npcID], pos, Quaternion.identity) as GameObject;


        return clone;

    }
}
