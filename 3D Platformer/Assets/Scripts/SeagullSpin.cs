using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullSpin : MonoBehaviour
{
    [SerializeField] float FlySpeed =1f;


    private void Update()
    {
        transform.Rotate(new Vector3(0,FlySpeed,0)* Time.deltaTime);
    }
}
