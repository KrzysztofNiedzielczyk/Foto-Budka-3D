using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhotoManager : MonoBehaviour
{
    //list of imported assets
    private List<GameObject> folderObjList = new List<GameObject>();
    //list of objects in the scene
    private List<GameObject> scenbeObjList = new List<GameObject>();

    //foler to witch objects will be parented
    public GameObject objFolder;

    //currently active object
    [SerializeField]
    private Transform currentObj;

    //rotation parameters
    Vector3 prevPos = Vector3.zero;
    Vector3 posDelta = Vector3.zero;
    Quaternion originalRotation = Quaternion.identity;

    //additional options
    public bool returnOriginalRotation = true;

    private void Start()
    {
        LoadAssets();
    }

    private void Update()
    {
        //if mouse drag -> rotate current object
        if (Input.GetMouseButton(0))
        {
            Rotate();
        }
        //current mouse position
        prevPos = Input.mousePosition;
    }

    //loading up assets on the start
    public void LoadAssets()
    {
        //import assets from the folder and add them to the list
        folderObjList = Resources.LoadAll<GameObject>("Input").ToList();

        int i = 0;

        foreach (GameObject asset in folderObjList)
        {
            //spawn objects in the scene
            GameObject spawnedAsset = Instantiate(asset, objFolder.transform, objFolder);

            //leave only one object active
            spawnedAsset.SetActive(false);

            if (i == 0)
            {
                spawnedAsset.SetActive(true);
                currentObj = spawnedAsset.transform;
            }
            i++;

            //add instatniated objects to a new list
            scenbeObjList.Add(spawnedAsset);
        }
    }

    //Showing next object
    public void NextAsset()
    {
        int i = 0;
        foreach (GameObject obj in scenbeObjList)
        {
            //If object is not last on the list, going to the next one
            if (obj.activeInHierarchy == true && i != scenbeObjList.Count-1)
            {
                obj.SetActive(false);

                GameObject nextObj = scenbeObjList[i + 1];

                nextObj.SetActive(true);
                currentObj = nextObj.transform;

                //return to original rotation
                if (returnOriginalRotation)
                {
                    currentObj.rotation = originalRotation;
                }
                break;
            }

            //If object is last in the list, going back to the beginning
            if (i == scenbeObjList.Count - 1)
            {
                obj.SetActive(false);

                GameObject nextObj = scenbeObjList[0];

                nextObj.SetActive(true);
                currentObj = nextObj.transform;

                //return to original rotation
                if (returnOriginalRotation)
                {
                    currentObj.rotation = originalRotation;
                }
                break;
            }
            i++;
        }
    }

    //Showing previous object
    public void PreviousAsset()
    {
        foreach (GameObject obj in scenbeObjList)
        {
            //If object is not first on the list, going to the next one
            if (obj.activeInHierarchy == true && scenbeObjList.IndexOf(obj) != 0)
            {
                int i = scenbeObjList.IndexOf(obj);
                obj.SetActive(false);

                GameObject previousObj = scenbeObjList[i - 1];

                previousObj.SetActive(true);
                currentObj = previousObj.transform;

                //return to original rotation
                if (returnOriginalRotation)
                {
                    currentObj.rotation = originalRotation;
                }
                break;
            }

            //If object is first on the list, going to the end
            if (obj.activeInHierarchy == true && scenbeObjList.IndexOf(obj) == 0)
            {
                obj.SetActive(false);

                GameObject previousObj = scenbeObjList[scenbeObjList.Count - 1];

                previousObj.SetActive(true);
                currentObj = previousObj.transform;

                //return to original rotation
                if (returnOriginalRotation)
                {
                    currentObj.rotation = originalRotation;
                }
                break;
            }
        }
    }

    private void Rotate()
    {
        posDelta = Input.mousePosition - prevPos;

        //check if object is upside down and rotate it
        if (Vector3.Dot(currentObj.transform.up, Vector3.up) >= 0)
        {
            currentObj.Rotate(transform.up, -Vector3.Dot(posDelta, Camera.main.transform.right), Space.World);
        }
        else
        {
            currentObj.Rotate(transform.up, Vector3.Dot(posDelta, Camera.main.transform.right), Space.World);
        }

        currentObj.Rotate(Camera.main.transform.right, Vector3.Dot(posDelta, Camera.main.transform.up), Space.World);
    }

    public void MakeScreenshot()
    {
        ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
    }
}
