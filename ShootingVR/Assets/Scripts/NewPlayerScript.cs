using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject bullet;
    public GameObject gunRotation;
    public GameObject gunMuzzle;

    public float shootForce, upwardForce;
    public int magazineSize;
    int bulletsLeft, bulletShoot;

    public Vector3 collision = Vector3.zero;
    public LayerMask layer;


    public bool allowInvoke = true;
    private void Awake()
    {
        bulletsLeft = magazineSize;
    }
    void Start()
    {
        SerialManager.serialOpen();
    }

    void Update()
    {
        MyInput();

        
    }
    private void MyInput() //Adding reloading time and magazine count later...
    {
        
        if (SerialManager.checkTrigger())
        {
            Shoot();
        }
        /*
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        */
    }

    void Shoot()
    {
        bulletsLeft--;
        bulletShoot++;
        //Creating a new ray from the gun muzzle with a angle of the weapon
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

        //Calculate direction from the muzzle to the target point
        Vector3 direction = targetPoint - gunMuzzle.transform.position;


        //Adding a spread later...

        //Instantiate the bullet
        GameObject currentBullet = Instantiate(bullet, gunMuzzle.transform.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized;
        //Adding force to the bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

        
        
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(collision, 0.2f);
    }
}
