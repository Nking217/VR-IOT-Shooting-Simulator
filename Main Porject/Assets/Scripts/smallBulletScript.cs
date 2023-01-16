using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class smallbulletScript : MonoBehaviour
{
    public Vector3 TargetPosition;
    public GameObject bullet;



    //public Transform target;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkHit();
    }
  

    void checkHit()
    {
        float distance = Vector3.Distance(bullet.transform.position, TargetPosition);
        Debug.Log(distance);
        if (distance <= 1)
        {
            SerialManager.sendHitConfirmation();
        }
    }



}
