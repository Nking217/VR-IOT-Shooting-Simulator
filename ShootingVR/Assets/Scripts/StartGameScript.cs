using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SerialManager.serialOpen();
    }

    // Update is called once per frame
    void Update()
    {
        if (SerialManager.checkGameStart())
        {
            SceneManager.LoadScene("SimpleNaturePack_Demo");
        }
    }
    // SceneManager.LoadScene (sceneName:"Put the name of the scene here");

}
