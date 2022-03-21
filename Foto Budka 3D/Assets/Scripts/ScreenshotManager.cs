using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    public void TakeScreenshot()
    {
        string timeNow = DateTime.Now.ToString("dd-MMMM-yyyy HHmmss");
        ScreenCapture.CaptureScreenshot(Path.Combine("/Assets/Resources/Output", "Screenshot " + timeNow + ".png"));
    }
}
