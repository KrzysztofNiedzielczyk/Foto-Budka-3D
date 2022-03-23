using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    //helper script that render camera scren textures and saves it to file

    private static ScreenshotHandler instance;

    public Camera myCamera;

    private bool takeScreenshotOnNextFrame;

    private bool changeFormat = true;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(takeScreenshotOnNextFrame == false)
        {
            StopCoroutine(OnRender());
        }
    }

    IEnumerator OnRender()
    {
        yield return new WaitForEndOfFrame();

        if (takeScreenshotOnNextFrame == true)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            string timeNow = DateTime.Now.ToString("dd-MMMM-yyyy HHmmss");

            //choose what format, take a screenshot and save it
            if (changeFormat)
            {
                byte[] byteArrayPNG = renderResult.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Output/Screenshot " + timeNow + ".png", byteArrayPNG);
                Debug.Log("Saved CameraScreenshot.png");
            }
            else if (!changeFormat)
            {
                byte[] byteArrayJPG = renderResult.EncodeToJPG();
                System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Output/Screenshot " + timeNow + ".jpg", byteArrayJPG);
                Debug.Log("Saved CameraScreenshot.jpg");
            }

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
        StartCoroutine(OnRender());
    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }

    public void ChangeFormat()
    {
        if (changeFormat)
            changeFormat = false;
        else
            changeFormat = true;
    }
}
