using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    public float lifeTime = 1;
    private float currentLifeTime = 1;
    public bool destroy = false;
    // Start is called before the first frame update
    private void OnEnable()
    {
        currentLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLifeTime > 0)
        {
            currentLifeTime -= Time.deltaTime;
        }
        else
        {
            if (destroy)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
           
        }
       
    }
}
