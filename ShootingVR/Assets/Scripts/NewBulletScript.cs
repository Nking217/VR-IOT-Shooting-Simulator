using System;
using TMPro;
using UnityEngine;

public class NewBulletScript : MonoBehaviour
{
    public GameObject lastCollision;
    [Range(0f, 1f)]
    public float bodyShotCenterRadius, bodyShotWideRadius, headShotRadius; //Setting radius for hitting areas on target
    public TextMeshPro lastHitText;
    public Rigidbody rb;
    public GameObject explosion;
    GameObject particleObject;
    public GameObject bullet;
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    public int explosionDamage; //Useless
    public float explosionRange;

    public int maxColissions;
    public float maxLifetime;
    public bool explodeOnTouch = true;
    int collisions;
    PhysicMaterial physics_mat;

    // Start is called before the first frame update
    void Start()
    {
        setup();
        
    }

 
    // Update is called once per frame
    void Update()
    {
        //When to explode:
        if(collisions > maxColissions) Explode();
        
        //Count down lifetime
        maxLifetime -= Time.deltaTime;
        if(maxLifetime <= 0) Explode();
    }

    private void setup()
    {
        //Create a new physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    private void Explode()
    {
        //Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);
        
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        //Debug.Log("Delay has finished");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;
        if (collision.collider.CompareTag("Target")) //Check if the bullet has hit a target
        {
            GameManager.totalHits++; //Adding up target hits for scoreing
            lastCollision = collision.collider.gameObject;
            checkScore(lastCollision); //Running function for checking the score if you hit a target

            //Run function for score
        }
    }
 
    void checkScore(GameObject targetObject)
    {
        Vector3 headShotPosition = targetObject.transform.GetChild(1).transform.position;
        Vector3 bodyShotPosition = targetObject.transform.GetChild(0).transform.position;
        float distanceFromHead = Vector3.Distance(gameObject.transform.position, headShotPosition);
        float distanceFromBody = Vector3.Distance(gameObject.transform.position, bodyShotPosition);
        if(distanceFromHead <= headShotRadius)
        {
            Debug.Log("Head Shot");
            GameManager.headShotHits++;
            lastHitText.SetText("Last Hit: Head");
            Debug.Log("Total Head Hits: " + GameManager.headShotHits);
            SerialManager.sendHitConfirmation();
            //Add one headShot Point
        }
        if(distanceFromBody <= bodyShotWideRadius)
        {
            if(distanceFromBody > bodyShotCenterRadius) {
                //Add one wide radius point
                Debug.Log("Body shot, wide.");
                GameManager.centerBodyHits++;
                lastHitText.SetText("Last Hit: Center Body");
                Debug.Log("Total Body Center Hits: " + GameManager.centerBodyHits);
            }
            else
            {
                Debug.Log("Body shot, center mass.");
                //Add one small radius point
                GameManager.wideBodyHits++;
                lastHitText.SetText("Last Hit: Wide Body");
                Debug.Log("Total Body Wide Hits: " + GameManager.wideBodyHits);
            }
        }
    }

}
