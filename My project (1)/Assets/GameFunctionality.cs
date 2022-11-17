using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using System.IO.Ports;
using System;
using System.Diagnostics;
using System.Collections;
using System.Linq.Expressions;

public class GameFunctionality : MonoBehaviour
{
    public GameObject prefab;
    public Transform tower_position;
    //public float speed = 6f;
    SerialPort serialPort = new SerialPort("COM3", 9600);
    int val = 0;

    // Start is called before the first frame update
    void Start()
    {
        serialPort.Open();
        serialPort.ReadTimeout = 1;
    }


    

    // Update is called once per frame
    void Update()
    {
        try
        {
            string readvalue = serialPort.ReadLine();
            val = int.Parse(readvalue);
            if(val == 1)
            {
                GameObject bullet = Instantiate(prefab, new Vector3(tower_position.position.x - 1.3f, tower_position.position.y + 2.2f, tower_position.position.z), Quaternion.identity);
                bullet.AddComponent<Rigidbody>();
                //Apply a force to this Rigidbody in direction of this GameObjects up axis
                Rigidbody bullet_rigidbody = bullet.GetComponent<Rigidbody>();
                bullet_rigidbody.AddForce(-1 * transform.right * 2500f);
                val = 0;
            }
        }
        catch (TimeoutException e)
        {

        }
        
        /*
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(prefab, new Vector3(tower_position.position.x-1.3f, tower_position.position.y+2.2f, tower_position.position.z), Quaternion.identity);
            bullet.AddComponent<Rigidbody>();
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            Rigidbody bullet_rigidbody = bullet.GetComponent<Rigidbody>();
            bullet_rigidbody.AddForce(-1 * transform.right * 2500f);
        }
        */
        
    }

    

    


    
}