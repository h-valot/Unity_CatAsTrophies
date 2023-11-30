using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public CanvasRenderer firstElement;
    public CanvasRenderer lastElement;
    public GameObject leftArrow;
    public GameObject rightArrow;
    


    // Update is called once per frame
    void Update()
    {
        if(firstElement.cull) 
        {
            leftArrow.SetActive(true);
            
        }
        else
        {
            leftArrow.SetActive(false);
        }

        if (lastElement.cull)
        {
            rightArrow.SetActive(true);

        }
        else
        {
            rightArrow.SetActive(false);
        }
    }
}
