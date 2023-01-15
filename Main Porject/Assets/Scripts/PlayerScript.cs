using BigRookGames.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject prefab;
    public Transform gunrotation;
    public Transform muzzlepos;
    public float bulletSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        SerialManager.serialOpen();
    }

    // Update is called once per frame
    void Update()
    {
        // play this function only when real-button pushed.
        //gun.GetComponent<GunfireController>().FireWeapon(); //Activates the fire effect and the sound. by the asset terms.

        if (SerialManager.checkTrigger())
        {
            
            GameObject bullet = Instantiate(prefab, muzzlepos.position, gunrotation.rotation);
            bullet.GetComponent<Rigidbody>().velocity = muzzlepos.transform.forward * bulletSpeed;
            //bullet.AddComponent<Rigidbody>();
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            Rigidbody bullet_rigidbody = bullet.GetComponent<Rigidbody>();
            
            gun.GetComponent<GunfireController>().FireWeapon(); //Activates the fire effect and the sound. by the asset terms. Fire Sound
            
            //bullet_rigidbody.AddForce(-1 * transform.forward * 2500f); //Applies force to the bullet (firing it from the muzzle)
            //bullet_rigidbody.AddForce(transform.up * 400f);
        }
        else if (SerialManager.checkGameStop())
        {
            SceneManager.LoadScene("Home Screen");
        }
    }

    

}
