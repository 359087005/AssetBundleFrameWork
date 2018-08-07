using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AssetBundleFrameWork
{
    /// <summary>
    /// 主流程第四层  通过调用辅助类 abmanifestLoader  读取unity提供的manifest文件
    /// 所有场景的AB的管理
    /// 以场景为单位
    /// 
    /// 功能：  读取“Manifest”清单文件，缓存脚本。
    ///         以场景为单位，管理整个项目的所有AB包。
    /// </summary>
    public class AssetBundleMgr :MonoBehaviour
    {
        //单例
        private static AssetBundleMgr _Instance;
        //所有的场景集合
        private Dictionary<string, MultiABMgr> _DicAllScenes = new Dictionary<string, MultiABMgr>();
        //系统类  清单文件
        private AssetBundleManifest _ManifestObj = null;

        private AssetBundleMgr(){}

        /// <summary>
        /// 得到本类实例
        /// </summary>
        /// <returns></returns>
        public static AssetBundleMgr GetInstance()
        {
            if(_Instance == null)
            {
                _Instance = new GameObject("AssetBundleMgr").AddComponent<AssetBundleMgr>();
            }
            return _Instance;
        }

        private void Awake()
        {
            //加载清单文件
            StartCoroutine(ABMainfestLoader.GetInstance().LoadMainifestFile());
        }

        /// <summary>
        /// 下载AB指定包
        /// </summary>
        /// <param name="scenesName"></param>
        /// <param name="abName"></param>
        /// <param name="loadAllCompleteHandle">调用是否完成</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundlePack(string scenesName,string abName,DeLoadComplete loadAllCompleteHandle)
        {
            //参数检查
            if (string.IsNullOrEmpty(scenesName) || string.IsNullOrEmpty(abName))
            {
                Debug.LogError("LoadAssetBundlePack方法加载错误，场景名或AB包名称错误");
                yield return null;
            }
            //等待manifest 清单文件加载完成
            while (!ABMainfestLoader.GetInstance().IsLoadCompleted)
            {
                yield return null;
            }
            _ManifestObj = ABMainfestLoader.GetInstance().GetABManifest();
            if (_ManifestObj == null)
            {
                Debug.LogError("LoadAssetBundlePack方法加载错误,_ManifestObj为空");
                yield return null;
            }
            //当前场景加入集合
            if (!_DicAllScenes.ContainsKey(scenesName))
            {
                MultiABMgr multiObj = new MultiABMgr(scenesName,abName, loadAllCompleteHandle);
                _DicAllScenes.Add(scenesName,multiObj);
            }

            //调用下一层  多包管理类
            MultiABMgr tmpMultiObj = _DicAllScenes[scenesName];
            if (tmpMultiObj == null)
            {
                Debug.LogError("LoadAssetBundlePack方法加载错误,tmpMultiObj为空");
            }
            //调用多包管理类  加载指定AB包  
            yield return tmpMultiObj.LoadAssetBundle(abName);
        }

        /// <summary>
        /// 加载AB包中资源
        /// </summary>
        /// <param name="scenesName"></param>
        /// <param name="abName">ab名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否使用缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string scenesName, string abName,string assetName, bool isCache)
        {
            if (_DicAllScenes.ContainsKey(scenesName))
            {
                MultiABMgr multObj = _DicAllScenes[scenesName];
                return multObj.LoadAsset(abName,assetName,isCache);
            }
            Debug.LogError("LoadAsset方法加载错误，找不到场景名称,资源加载失败. SceneName = "+scenesName);
            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="scenesName"></param>
        public void DisposeAllAssets(string scenesName)
        {
            if (_DicAllScenes.ContainsKey(scenesName))
            {
                MultiABMgr multObj = _DicAllScenes[scenesName];
                multObj.DisPoseAllAsset();
            }
            else
            {
                Debug.LogError("DisposeAllAssets方法加载错误，找不到场景名称. SceneName = " + scenesName);
            }
        }
    }
}
