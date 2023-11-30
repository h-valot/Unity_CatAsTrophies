using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBonfireManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Registry.isInitialized)
        {
            SceneManager.LoadScene("Init");
            return;
        }
    }


}
