using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using System.IO.Ports;
using System;
using System.Collections;



public class GameFunctionality : MonoBehaviour
{
    public GameObject prefab;
    public Transform tower_position;
    public static SerialPort serialPort1 = new SerialPort("COM3", 9600);
    

    //Setup of the constant chars for the serial communication
    //public string endOfReturnString = "\0";
    //public string endOfCommandString = "$";


    // Start is called before the first frame update
    void Start()
    {
        serialPort1.Open(); //Opening the serial port
        serialPort1.ReadTimeout = 1; //Setting a read timeout for the serial port. making sure that the main function wont be stuck.
        serialPort1.WriteTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            string incomingString = serialPort1.ReadLine(); //Reading from the serial port to a string value.
            //Debug.Log(incomingString);
            if(incomingString == "FIRE") //if the value is on the canon will shoot one bullet
            {
                GameObject bullet = Instantiate(prefab, new Vector3(tower_position.position.x - 1.3f, tower_position.position.y + 2.2f, tower_position.position.z), Quaternion.identity);
                bullet.AddComponent<Rigidbody>();
                //Apply a force to this Rigidbody in direction of this GameObjects up axis
                Rigidbody bullet_rigidbody = bullet.GetComponent<Rigidbody>();
                bullet_rigidbody.AddForce(-1 * transform.right * 2500f);
                bullet_rigidbody.AddForce(transform.up * 500f);
                incomingString = null;
            }

        }
        catch (TimeoutException e)
        {
            //the ESP is not sending anything at the moment, timeout has passed, will try to get any value in each frame.
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
