using BigRookGames.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NewPlayerScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject bullet;
    public GameObject gunRotation;
    public GameObject gunMuzzle;

    public float shootForce, upwardForce;
    public float spread, timeBetweenShotting, reloadTime, timeBetwwenShots;
    public int magazineSize, bulletsPerTap;
    int bulletsLeft, bulletShot;
    
    public Vector3 collision = Vector3.zero;
    public LayerMask layer;

    bool shooting, readyToShoot, reloading;


    public TextMeshPro ammunationDisplay;

    public bool allowInvoke = true;
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    void Start()
    {
        SerialManager.serialOpen();
    }

    void Update()
    {
        MyInput();
        

        //Debug.Log(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        
        //Set Ammo display
        if(ammunationDisplay != null)
        {
            ammunationDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }                                                                                                                                                              
        
    }
    private void MyInput() //Adding reloading time and magazine count later...
    {

        
        shooting = SerialManager.checkTrigger(); //Input from ESP
        

        //Shooting
        //shooting = Input.GetKeyDown(KeyCode.Space); //Keyboad test input for shooting

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletShot = 0;
            Shoot();
        }

        //Reloading
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Automatic reload when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
    }

    void Shoot()
    {
        readyToShoot = false;

        //Creating a new ray from the gun muzzle with a angle of the weapon
        gun.GetComponent<GunfireController>().FireWeapon();
        Ray ray = new Ray(gunMuzzle.transform.position, gunRotation.transform.forward);
        RaycastHit hit;
        Vector3 targetPoint;

        //Checks if the ray is hitting something
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            collision = hit.point; //Debug...
    
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        //Calculate direction from the muzzle to the target point (without spread)
        Vector3 direction = targetPoint - gunMuzzle.transform.position;


        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate direction from the muzzle to the target point with spread
        Vector3 directionWithSpread = direction + new Vector3(x, y, 0); //direction + spread

        //Instantiate the bullet
        GameObject currentBullet = Instantiate(bullet, gunMuzzle.transform.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized; //rotate the bullet to the shooting direction

        //Instantiate the bullet with spread
        /*
        GameObject currentBullet = Instantiate(bullet, gunMuzzle.transform.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        */


        //Adding force to the bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

        /*
        //Currently not adding force with spread
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        */

        bulletsLeft--;
        bulletShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShotting);
            allowInvoke = false;
        }
        
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(collision, 0.2f);
    }

    
}
