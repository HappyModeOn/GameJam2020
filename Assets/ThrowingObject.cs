using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObject : MonoBehaviour
{
    public GameObject groundVFX;
    public GameObject waterVFX;
    public int damge = 1;
    [HideInInspector]
    public float directionZ = 0;

    public Vector2 minMaxX;
    public Vector2 minMaxY;
    public Vector2 minMaxZ;

    float rotationRandom = 30;
    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 randomRotation = new Vector3(0, 0, Random.Range(-rotationRandom, rotationRandom));
        GetComponent<Rigidbody>().AddRelativeTorque(randomRotation, ForceMode.Impulse);
        if (directionZ == -1)
        {
            GetComponent<Rigidbody>().AddForce(Random.Range(minMaxX.x, minMaxX.y), Random.Range(minMaxY.x, minMaxY.y), Random.Range(-minMaxZ.x, -minMaxZ.y));
        }
        else if (directionZ == 1)
        {
            GetComponent<Rigidbody>().AddForce(Random.Range(minMaxX.x, minMaxX.y), Random.Range(minMaxY.x, minMaxY.y), Random.Range(minMaxZ.x, minMaxZ.y));

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Water")
        {
            if (waterVFX != null)
            {
                if (waterVFX.activeInHierarchy == false)
                {
                    waterVFX.transform.parent = null;
                    waterVFX.transform.position = transform.position;
                    waterVFX.transform.rotation = new Quaternion();
                    waterVFX.SetActive(true);
                }
            }
        }
        else
        {
            /*
            if (groundVFX != null)
            {
                if (groundVFX.activeInHierarchy == false)
                {
                    groundVFX.SetActive(true);
                }
            }
            */
        }
       
    }
}
