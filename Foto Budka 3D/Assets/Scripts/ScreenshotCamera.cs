using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCamera : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<Camera>().fieldOfView = Camera.main.fieldOfView;
    }
}
