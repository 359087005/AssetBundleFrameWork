using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFrameWork
{
    /// <summary>
    /// 单一资源AB下载
    ///     WWW下载AB
    /// </summary>
    public class SingleABLoader : System.IDisposable
    {
        /// <summary>
        /// 引用AB管理类
        /// </summary>
        private AssetBundleLoader _ABLoader;
        /// <summary>
        /// AB 名称
        /// </summary>
        private string _ABName;
        /// <summary>
        /// 下载路径
        /// </summary>
        private string _ABDownLoadPath;

        /// <summary>
        /// WWW下载完毕调用委托加载
        /// </summary>
        private DeLoadComplete _LoadCompleteHandle;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abName"></param>
        public SingleABLoader(string abName, DeLoadComplete loadComplete)
        {
            _ABLoader = null;
            this._ABName = abName;
            //委托初始化
            _LoadCompleteHandle = loadComplete;
            //AB包下载路径
            _ABDownLoadPath = PathTools.GetWWWPath() + "/" + _ABName;
        }
        /// <summary>
        /// 通过www加载AB资源包
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle()
        {
            using (WWW www = new WWW(_ABDownLoadPath))
            {
                yield return www;
                ///WWW下载进度
                if (www.progress >= 1)
                {
                    //获取AB实例
                    AssetBundle abObj = www.assetBundle;
                    if (abObj != null)
                    {
                        _ABLoader = new AssetBundleLoader(abObj);
                        //...
                        if (_LoadCompleteHandle != null)
                        {
                            //执行委托
                            _LoadCompleteHandle(_ABName);
                        }
                    }
                    else
                    {
                        Debug.LogError("LoadAssetBundle方法下载失败，路径" + _ABDownLoadPath + " " + www.error);
                    }
                }
            }
        }

        /// <summary>
        /// 加载AB包内的资源   注意加载资源包,加载包资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache)
        {
            if (_ABLoader != null)
            {
                return _ABLoader.LoadAsset(assetName, isCache);
            }
            Debug.LogError("LoadAsset方法加载包内资源错误,该资源为" + assetName);

            return null;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset"></param>
        public void UnLoadAsset(UnityEngine.Object asset)
        {
            if (_ABLoader != null)
            {
                _ABLoader.unLoadAsset(asset);
            }
            else
            {
                Debug.LogError("UnLoadAsset方法卸载错误，资源名为" + asset.name);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_ABLoader != null)
            {
                _ABLoader.Dispose();
                _ABLoader = null;
            }
            else
            {
                Debug.LogError("Dispose方法无法释放资源，因为_ABLoader为空");
            }
        }

        /// <summary>
        /// 释放当前ab资源包 且卸载所有资源
        /// </summary>
        public void DisposeAll()
        {
            if (_ABLoader != null)
            {
                _ABLoader.DisposeAll();
                _ABLoader = null;
            }
            else
            {
                Debug.LogError("Dispose方法无法释放资源，因为_ABLoader为空");
            }
        }

        /// <summary>
        /// 查询当前AB包中所有的资源
        /// </summary>
        public string[] RetrivalAllAssetName()
        {
            if (_ABLoader != null)
            {
                return _ABLoader.RetriveAllAssetName();
            }
            Debug.LogError("Dispose方法无法释放资源，因为_ABLoader为空");
            return null;
        }
    }
}
