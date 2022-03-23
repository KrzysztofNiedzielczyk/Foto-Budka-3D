using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.IO;

public class PhotoManager : MonoBehaviour
{
    //list of imported assets
    private List<GameObject> folderObjList = new List<GameObject>();
    //list of objects in the scene
    private List<GameObject> scenbeObjList = new List<GameObject>();

    //foler to witch objects will be parented
    [SerializeField]
    private GameObject objFolder;

    //currently active object
    private Transform currentObj;

    //rotation parameters
    Vector3 prevPos = Vector3.zero;
    Vector3 posDelta = Vector3.zero;
    Quaternion originalRotation = Quaternion.identity;

    public bool rotationY = true;
    public bool rotationX = true;
    public bool rotationZ = true;

    //zoom
    private float minFov = 10f;
    private float maxFov = 130f;
    private float sensitivity = 20f;

    //additional options
    public bool returnOriginalRotation = true;

    private void Awake()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/Output"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Output");
        }
    }

    private void Start()
    {
        LoadAssets();
    }

    private void Update()
    {
        //if mouse clicked -> rotate current object
        if (Input.GetMouseButton(0) && !IsMouseOverUI())
        {
            RotateWorld();
        }
        //if mouse clicked -> rotate current object
        if (Input.GetMouseButton(1) && !IsMouseOverUI())
        {
            RotateSelf();
        }
        //current mouse position
        prevPos = Input.mousePosition;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Zoom();
        }
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

    private void RotateWorld()
    {
        posDelta = Input.mousePosition - prevPos;

        currentObj.Rotate(transform.up, -Vector3.Dot(posDelta, Camera.main.transform.right), Space.World);

        currentObj.Rotate(Camera.main.transform.right, Vector3.Dot(posDelta, Camera.main.transform.up), Space.World);
    }

    private void RotateSelf()
    {
        posDelta = Input.mousePosition - prevPos;

        currentObj.Rotate(transform.up, -Vector3.Dot(posDelta, Camera.main.transform.right), Space.Self);

        currentObj.Rotate(Camera.main.transform.right, Vector3.Dot(posDelta, Camera.main.transform.up), Space.Self);
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void MakeScreenshot()
    {
        ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
    }

    private void Zoom()
    {
        float fov = Camera.main.fieldOfView;
        fov += -Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
}
