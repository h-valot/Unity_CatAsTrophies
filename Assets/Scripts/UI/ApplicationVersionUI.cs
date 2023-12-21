using TMPro;
using UnityEngine;

public class ApplicationVersion : MonoBehaviour
{
    [Header("REFERENCE")]
    public TextMeshProUGUI buildTM;

    private void OnEnable() => buildTM.text = $"App version: {Application.version}";
}