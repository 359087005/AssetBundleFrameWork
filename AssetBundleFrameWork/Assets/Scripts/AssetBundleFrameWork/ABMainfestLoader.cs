using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AssetBundleFrameWork
{
    /// <summary>
    /// 读取ab的关系
    /// </summary>
    public class ABMainfestLoader : System.IDisposable
    {
        private ABMainfestLoader()
        {
            _StrManifestPath = PathTools.GetWWWPath() + "/" + PathTools.GetPlatformName();
            _ManifestObj = null;
            _ABReadManifest = null;
            _IsLoadCompleted = false;
        }

        #region Define
        private static ABMainfestLoader _Instance;
        public static ABMainfestLoader GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ABMainfestLoader();
            }
            return _Instance;
        }
        /// <summary>
        /// 清单文件系统类
        /// </summary>
        private AssetBundleManifest _ManifestObj;
        /// <summary>
        /// 清单文件路径
        /// </summary>
        private string _StrManifestPath;
        /// <summary>
        /// 读取AB清单文件的AB
        /// </summary>
        private AssetBundle _ABReadManifest;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        private bool _IsLoadCompleted;
        public bool IsLoadCompleted
        {
            get { return _IsLoadCompleted; }
        }
        #endregion

        /// <summary>
        /// 加载manifest清单文件
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadMainifestFile()
        {
            using (WWW www = new WWW(_StrManifestPath))
            {
                yield return www;
                if (www.progress >= 1)
                {
                    //加载完成
                    AssetBundle abObj = www.assetBundle;
                    if (abObj != null)
                    {
                        _ABReadManifest = abObj;
                        _ManifestObj = _ABReadManifest.LoadAsset(ABDefine.AB_MANIFEST) as AssetBundleManifest;  
                        _IsLoadCompleted = true;
                    }
                    else
                    {
                        Debug.LogError("LoadMainifestFile加载为空,Path = "+ _StrManifestPath + "    " + www.error);
                    }
                }
            }
        }

        /// <summary>
        /// 获取abManifest系统类实例
        /// </summary>
        /// <returns></returns>
        public AssetBundleManifest GetABManifest()
        {
            if (_IsLoadCompleted)
            {
                if (_ManifestObj != null)
                    return _ManifestObj;
                else
                    Debug.LogError("LoadMainifestFile获取错误,值为空");
            }
            else
            {
                Debug.LogError("LoadMainifestFile获取错误,加载未完毕。");
            }
            return null;
        }

        /// <summary>
        /// 获取abManifest系统类实例 指定包名称依赖项
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public string[] RetrivalDependce(string abName)
        {
            if (_ManifestObj != null&& !string.IsNullOrEmpty(abName))
            {
                return _ManifestObj.GetAllDependencies(abName);
            }
            return null;
        }
        /// <summary>
        /// 释放本类资源   
        /// </summary>
        public void Dispose()
        {
            if (_ABReadManifest != null)
            {
                _ABReadManifest.Unload(true);
            }
        }
    }
}

