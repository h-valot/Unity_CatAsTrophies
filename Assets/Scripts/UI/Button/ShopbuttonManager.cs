using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopbuttonManager : MonoBehaviour
{
    public GameObject featureButton;
    public GameObject catsButton;
    public GameObject boostButton;
    public GameObject tunaButton;

    public void OpenFeaturedPanel()
    {
        featureButton.SetActive(true);
        catsButton.SetActive(false);
        boostButton.SetActive(false);
        tunaButton.SetActive(false);
        Debug.Log("Je suis dans le panel Featured");
    }
    public void OpenCatsPanel()
    {
        featureButton.SetActive(false);
        catsButton.SetActive(true);
        boostButton.SetActive(false);
        tunaButton.SetActive(false);
        Debug.Log("Je suis dans le panel Cats");
    }
    public void OpenBoostPanel()
    {
        featureButton.SetActive(false);
        catsButton.SetActive(false);
        boostButton.SetActive(true);
        tunaButton.SetActive(false);
        Debug.Log("Je suis dans le panel Boost");
    }
    public void OpenTunaPanel()
    {
        featureButton.SetActive(false);
        catsButton.SetActive(false);
        boostButton.SetActive(false);
        tunaButton.SetActive(true);
        Debug.Log("Je suis dans le panel Tuna");
    }
}
