using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AssetLoader : MonoBehaviour
{
    public List<Object> objList = new List<Object>();

    private void Start()
    {
        LoadAssets();
    }

    public void LoadAssets()
    {
        objList = Resources.LoadAll<Object>("3D Objects").ToList();
    }
}
