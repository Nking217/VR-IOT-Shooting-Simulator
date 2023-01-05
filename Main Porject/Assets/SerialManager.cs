using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using System.IO.Ports;
using System;


public class SerialManager : MonoBehaviour
{
    public static SerialPort serialPort1 = new SerialPort("COM3", 115200);
    
    // Start is called before the first frame update
    void Start()
    {
        serialOpen();
        Debug.Log("SerialManager has started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void serialOpen() //Function for opening the serial port
    {
        if (!serialPort1.IsOpen)
        {
            serialPort1.Open();
            serialPort1.ReadTimeout = 1;
            serialPort1.WriteTimeout = 1;
            Debug.Log("Serial Port has opened");
        }
        else
        {
            Debug.Log("Serial Port has opened");
        }
        
    }

    public static void serialClose() //Function for closing the seiral port
    {
        serialPort1.Close();
    }


    public static string serialManagerRead() //Function for reading from serial port - returns string
    {
        string var;
        try
        {
            var = serialPort1.ReadLine();
        }
        catch(TimeoutException e)
        {
            var = null;
        }
        return var;
    }

    public static void serialManagerWrite(string var) //Function for sending data to the serial port
    {
        try
        {
            serialPort1.Write(var);
        }
        catch(TimeoutException)
        {

        }
    }
    public static void sendHitConfirmation() //Function for sending a hit confirmation to the serial port
    {
        serialManagerWrite("HIT");
        
    }

    public static bool checkTrigger() //Function for checking if the trigger was pressed
    {
        string var = serialManagerRead();
        if (var == "FIRE")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool checkGameStart()
    {
        string var = serialManagerRead();
        if(var == "START")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
