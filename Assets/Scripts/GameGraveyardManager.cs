using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGraveyardManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // load the init scene if it hasn't been loaded yet
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
