using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raytest : MonoBehaviour
{
    public GameObject lastHit;
    public Transform muzzlepose;
    public Transform gunrotation;
    public Vector3 collision = Vector3.zero;
    public LayerMask layer;
    
    // Start is called before the first frame update
    void Start()
    {
        //SerialManager.serialOpen();
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject bullet = Instantiate(prefab, muzzlepos.position, gunrotation.rotation, transform);
        var ray = new Ray(muzzlepose.position, gunrotation.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collision, 0.2f);
    }
}
