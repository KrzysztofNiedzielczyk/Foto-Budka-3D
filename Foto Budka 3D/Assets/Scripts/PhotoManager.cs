using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhotoManager : MonoBehaviour
{
    //list of imported assets
    public List<GameObject> folderObjList = new List<GameObject>();
    //list of objects in the scene
    public List<GameObject> scenbeObjList = new List<GameObject>();

    public GameObject objFolder;

    private void Start()
    {
        LoadAssets();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextAsset();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PreviousAsset();
        }
    }

    //loading up assets on the start
    public void LoadAssets()
    {
        //import assets from the folder and add them to the list
        folderObjList = Resources.LoadAll<GameObject>("3D Objects").ToList();

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
                scenbeObjList[i + 1].SetActive(true);
                break;
            }

            //If object is last in the list, going back to the beginning
            if (i == scenbeObjList.Count - 1)
            {
                obj.SetActive(false);
                scenbeObjList[0].SetActive(true);
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
                scenbeObjList[i - 1].SetActive(true);
                break;
            }

            //If object is first on the list, going to the end
            if (obj.activeInHierarchy == true && scenbeObjList.IndexOf(obj) == 0)
            {
                obj.SetActive(false);
                scenbeObjList[scenbeObjList.Count-1].SetActive(true);
                break;
            }
        }
    }
}
