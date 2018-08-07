using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssetBundleFrameWork
{
    /// <summary>
    /// AB资源管理类
    /// </summary>
    /// 1，管理与加载制定AB资源
    /// 2，加载具有  缓存功能 的资源 
    /// 3，卸载释放资源
    /// 4，查看当前资源
    public class AssetBundleLoader :System.IDisposable
    {
        /// <summary>
        /// 当前ab
        /// </summary>
        private AssetBundle currentAssetBundle;
        /// <summary>
        /// 缓存器
        /// </summary>
        private Hashtable ht;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abObj">给定WWW  加载的 AB 实例</param>
        public AssetBundleLoader(AssetBundle abObj)
        {
            if (abObj != null)
                currentAssetBundle = abObj;
            ht = new Hashtable();
        }

        #region 加载资源
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache = false)
        {
            return LoadResources<UnityEngine.Object>(assetName,isCache);
        }
        /// <summary>
        /// 加载当前AB包资源 带缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        private T LoadResources<T>(string assetName, bool isCache) where T : UnityEngine.Object
        {
            //是否缓存集合已存在
            if (ht.Contains(assetName))
            {
                return ht[assetName] as T;
            }
            //正是加载
           T tmpTResources =  currentAssetBundle.LoadAsset<T>(assetName);
            //加入缓存集合
            if (tmpTResources != null && isCache)
            {
                ht.Add(assetName, tmpTResources);
            }
            else if (tmpTResources == null)
            {
                Debug.LogError("LoadResources<T>方法无法获取该资源" + assetName);
            }

            return tmpTResources;
        }
        #endregion

        #region 卸载资源

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public bool unLoadAsset(UnityEngine.Object asset)
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);//不管使用或不适用均卸载
                return true;
            }

            Debug.Log("unLoadAsset方法 资源为空 ，无法卸载。" + asset);
            return false;
        }


        /// <summary>
        /// 卸载当前AB 镜像资源
        /// </summary>
        public void Dispose()
        {
            currentAssetBundle.Unload(false);
        }
        /// <summary>
        ///  卸载当前AB 镜像资源 且释放内存资源
        /// </summary>
        public void DisposeAll()
        {
            currentAssetBundle.Unload(true);
        }

        #endregion

        #region 查看资源

        /// <summary>
        ///  查询当前AB中包含的所有资源名称
        /// </summary>
        /// <returns></returns>
        public string[] RetriveAllAssetName()
        {
            return currentAssetBundle.GetAllAssetNames();
        }
        #endregion
    }
}


