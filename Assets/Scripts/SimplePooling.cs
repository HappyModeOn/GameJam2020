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
    public GameObject Show(Vector3 position)
    {
        
        pool[poolID].transform.position = position;

        pool[poolID].SetActive(true);
        poolID++;

        if (poolID >= pool.Length)
        {
            poolID = 0;
        }

        return pool[poolID];
    }

}
