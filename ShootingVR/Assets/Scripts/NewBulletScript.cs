using UnityEngine;

public class NewBulletScript : MonoBehaviour
{
    
    public Rigidbody rb;
    public GameObject explosion;
    GameObject particleObject;
    //public GameObject targetPosition;
    public Vector3 targetPosition;
    public GameObject bullet;
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    ParticleSystem particleSystem;
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
        
        if (explosion != null)
        {
            particleObject = Instantiate(explosion, transform.position, Quaternion.identity);
            particleSystem = particleObject.GetComponent<ParticleSystem>();
        }
        


        //Instantiate explosion
        //if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);
        
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Debug.Log("Delay has finished");
        checkHit();
        Destroy(gameObject);
        particleSystem.Stop();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;
    }
    void checkHit()
    {
        float distance = Vector3.Distance(gameObject.transform.position, targetPosition);
        //Debug.Log(distance);
        if (distance <= 1)
        {
            //SerialManager.sendHitConfirmation();
            GameManager.totalPoints += 1;
            Debug.Log("elad muzar: " + GameManager.totalPoints);


        }
    }
}
