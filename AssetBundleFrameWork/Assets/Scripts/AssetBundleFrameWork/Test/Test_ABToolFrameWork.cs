using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 整体框架验证测试
/// </summary>

namespace AssetBundleFrameWork
{
    public class Test_ABToolFrameWork : MonoBehaviour
    {
        private string _SceneName = "Scene_01";

        private string _AssetBundleName = "Scene_1/prefabs.ab";

        private string _AssetName = "Plane.prefab";

        private void Start()
        {
            //调用AB包 
            StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(_SceneName,_AssetBundleName, LoadAllABCompleted));
        }
        //回调函数 所有的AB包都已经加载完毕
        void LoadAllABCompleted(string abName)
        {
            UnityEngine.Object tmpObj = null;
            tmpObj = AssetBundleMgr.GetInstance().LoadAsset(_SceneName,_AssetBundleName,_AssetName,false);
            if (tmpObj != null)
            {
                Instantiate(tmpObj);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("卸载资源执行中");
                AssetBundleMgr.GetInstance().DisposeAllAssets(_SceneName);
            }
        }
    }
}
