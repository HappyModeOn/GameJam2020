using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{
    public GameObject[] npcPrefab;


    public GameObject AddNPC(Vector3 pos, int npcID)
    {
        pos += new Vector3(Random.Range(-1.5f, 1.5f), 1f, Random.Range(-1.5f, 1.5f));
        GameObject clone = Instantiate(npcPrefab[npcID], pos, Quaternion.identity) as GameObject;


        return clone;

    }
}
