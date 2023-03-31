using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheepCheep : MonoBehaviour
{
    [SerializeField] GameObject rotationPoint;
    [SerializeField] float swimSpeed = 1f;

    void Update()
    {
        Swim();
    }


    void Swim()
    {
        transform.RotateAround(rotationPoint.transform.position,
                               Vector3.forward,
                               swimSpeed * Time.deltaTime);
    }
}