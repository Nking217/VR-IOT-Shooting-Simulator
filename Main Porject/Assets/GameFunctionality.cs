using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using System.IO.Ports;
using System;




public class GameFunctionality : MonoBehaviour
{
    public GameObject prefab;
    public Transform tower_position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SerialManager.checkTrigger())
        {
            GameObject bullet = Instantiate(prefab, new Vector3(tower_position.position.x - 1.3f, tower_position.position.y + 2.2f, tower_position.position.z), Quaternion.identity);
            bullet.AddComponent<Rigidbody>();
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            Rigidbody bullet_rigidbody = bullet.GetComponent<Rigidbody>();
            bullet_rigidbody.AddForce(-1 * transform.right * 2500f);
            bullet_rigidbody.AddForce(transform.up * 500f);
        }
        
        /*
        // Code for shooting the ball from the cannon (if something happens...)
        GameObject bullet = Instantiate(prefab, new Vector3(tower_position.position.x - 1.3f, tower_position.position.y + 2.2f, tower_position.position.z), Quaternion.identity);
        bullet.AddComponent<Rigidbody>();
        //Apply a force to this Rigidbody in direction of this GameObjects up axis
        Rigidbody bullet_rigidbody = bullet.GetComponent<Rigidbody>();
        bullet_rigidbody.AddForce(-1 * transform.right * 2500f);
        //val = 0;
        */


    }
    

    /*
    void startCommunication() //Opening the serial port and sending a signal to the ESP to start the game.
    {
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
        }
        else
        {
            serialPort.Write("READY"); //Sending a message to the ESP to enable the game functionality. 
        }
    }
    */
}
