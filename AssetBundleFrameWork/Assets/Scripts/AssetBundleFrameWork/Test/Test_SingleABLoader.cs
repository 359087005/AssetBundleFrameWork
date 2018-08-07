using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssetBundleFrameWork
{
    public class Test_SingleABLoader : MonoBehaviour
    {
        private SingleABLoader _LoadObj = null;

        private string _ABName = "scene_1/prefabs.ab";

        private string _ABAssetName1 = "Sphere.prefab";
        private string _ABAssetName2 = "Cube.prefab";

        private string _ABName2 = "scene_1/textures.ab";
        private string _ABName3 = "scene_1/materials.ab";
        private string _ABAssetName3 = "Plane.prefab";//Cube.prefab
        #region Test1
        //private void Start()
        //{
        //    _LoadObj = new SingleABLoader(_ABName, LoadComplete);

        //    StartCoroutine(_LoadObj.LoadAssetBundle());
        //}

        //private void LoadComplete(string abName) 
        //{
        //    UnityEngine.Object tmpObj = _LoadObj.LoadAsset(_ABAssetName1, false);
        //    Instantiate(tmpObj);

        //    UnityEngine.Object tmpObj2 = _LoadObj.LoadAsset(_ABAssetName2, false);
        //    Instantiate(tmpObj2);
        //}
        #endregion

        #region Test2
        private void Start()
        {
            SingleABLoader _loadDependObj = new SingleABLoader(_ABName2, LoadComplete1);

            StartCoroutine(_loadDependObj.LoadAssetBundle());
        }

        private void LoadComplete1(string abName)
        {
            SingleABLoader sing1 = new SingleABLoader(_ABName3, LoadComplete2);

            StartCoroutine(sing1.LoadAssetBundle());
        }

        private void LoadComplete2(string abName)
        {
             _LoadObj = new SingleABLoader(_ABName, LoadComplete3);
            StartCoroutine(_LoadObj.LoadAssetBundle());
        }
        private void LoadComplete3(string abName)
        {
            UnityEngine.Object tmpObj = _LoadObj.LoadAsset(_ABAssetName3, false);
            Instantiate(tmpObj);
        }
        #endregion
    }
}


