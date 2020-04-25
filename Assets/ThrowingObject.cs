using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObject : MonoBehaviour
{
    public int damge = 1;
    [HideInInspector]
    public float directionZ = 0;

    public Vector2 minMaxX;
    public Vector2 minMaxY;
    public Vector2 minMaxZ;
    private void OnEnable()
    {
        if (directionZ == -1)
        {
            GetComponent<Rigidbody>().AddForce(Random.Range(minMaxX.x, minMaxX.y), Random.Range(minMaxY.x, minMaxY.y), Random.Range(-minMaxZ.x, -minMaxZ.y));
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(Random.Range(minMaxX.x, minMaxX.y), Random.Range(minMaxY.x, minMaxY.y), Random.Range(minMaxZ.x, minMaxZ.y));

        }

    }
}
