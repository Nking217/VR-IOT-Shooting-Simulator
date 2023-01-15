using BigRookGames.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // play this function only when real-button pushed.
        gun.GetComponent<GunfireController>().FireWeapon();
    }
}
