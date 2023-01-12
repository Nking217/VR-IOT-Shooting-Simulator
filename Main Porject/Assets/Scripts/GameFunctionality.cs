using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using System.IO.Ports;
using System;
using UnityEngine.SceneManagement;

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
            bullet_rigidbody.AddForce(transform.up * 400f);
        }
        else if (SerialManager.checkGameStop())
        {
            SceneManager.LoadScene("Home Screen");
        }
        SerialManager.serialManagerPrint();
        
        
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
}
