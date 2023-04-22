using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTestScript : MonoBehaviour
{
    [Range(0f, 1f)]
    public float headrange, bodycenterrange, bodywiderange;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.GetChild(0).transform.position, bodycenterrange);
        Gizmos.DrawWireSphere(transform.GetChild(0).transform.position, bodywiderange);
        Gizmos.DrawWireSphere(transform.GetChild(1).transform.position, headrange);
    }
}
