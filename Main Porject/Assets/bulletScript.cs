using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class bulletScript : MonoBehaviour
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
        float distance = Vector3.Distance(bullet.transform.position, TargetPosition);
        
        Debug.Log(distance);
        if(distance <= 2)
        {
            Destroy(bullet);
            //Hit confirmation...
            // serialPort.WriteLine("HIT");
           
        }
    }


    
}
