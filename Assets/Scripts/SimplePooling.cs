using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePooling : MonoBehaviour {


    public GameObject[] pool;
    public int poolID = 0;

    public void Clear()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i] != null)
            {
                pool[i].SetActive(false);
            }
            
        }
        poolID = 0;
    }
    public void Show(Vector3 position, string tag, int direction)
    {
        pool[poolID].transform.position = position;

        pool[poolID].tag = tag;
        pool[poolID].SetActive(true);
        pool[poolID].GetComponent<ThrowingObject>().directionZ = direction;
        poolID++;

        if (poolID >= pool.Length)
        {
            poolID = 0;
        }
    }

}
