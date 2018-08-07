using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFrameWork
{
    /// <summary>
    /// 一个场景中多个assetBundle 的管理  第三层
    /// 
    ///     1，获取AB包之间的依赖于引用关系
    ///     2，管理AssetBundle包之间的自动连锁（递归）加载机制
    /// </summary>
    public class MultiABMgr
    {
        public MultiABMgr(string sceneName, string abName, DeLoadComplete loadAllABPackageCompleteHandle)
        {
            _CurrentSceneName = sceneName;
            _CurrntABName = abName;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelation = new Dictionary<string, ABRelation>();

            _LoadAllABPackageCompleteHandle = loadAllABPackageCompleteHandle;
        }

        #region 变量定义
        //下层引用类  "单个AB包加载实现类"
        private SingleABLoader _CurrentSingleABLoader;
        //"AB包实现类" 缓存集合(缓存AB包，防止重复加载)
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;
        //当前场景（调试使用）
        private string _CurrentSceneName;
        //当前AB名称
        private string _CurrntABName;
        //AB包与对应依赖关系集合
        private Dictionary<string, ABRelation> _DicABRelation;
        //委托  所有AB包加载完成
        private DeLoadComplete _LoadAllABPackageCompleteHandle;
        #endregion

        /// <summary>
        /// 完成指定AB包调用
        /// </summary>
        /// <param name="abName">AB包名称</param>
        private void CompleteLoadAB(string abName)
        {
            Debug.Log("CompleteLoadAB方法执行完成，当前完成abName是" + abName);
            if (abName.Equals(_CurrntABName))
            {
                if (_LoadAllABPackageCompleteHandle != null)
                {
                    _LoadAllABPackageCompleteHandle(abName);
                }
            }
        }
      

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string abName)
        {
            //AB包关系的建立
            if (!_DicABRelation.ContainsKey(abName))
            {
                ABRelation abRelationObj = new ABRelation(abName);
                _DicABRelation.Add(abName, abRelationObj);
            }
            ABRelation tmpABReltation = _DicABRelation[abName];

            //得到指定AB包所有依赖关系（查询清单文件）
            string[] strDependenceArray = ABMainfestLoader.GetInstance().RetrivalDependce(abName);
            foreach (string item_Depence in strDependenceArray)
            {
                //添加 依赖项
                tmpABReltation.AddDependence(item_Depence);
                //添加引用项  递归调用
                yield return LoadReference(item_Depence,abName);
            }

            //真正加载AB包
            if (_DicSingleABLoaderCache.ContainsKey(abName))
            {
                yield return _DicSingleABLoaderCache[abName].LoadAssetBundle();
            }
            else
            {
                _CurrentSingleABLoader = new SingleABLoader(abName, CompleteLoadAB);
                _DicSingleABLoaderCache.Add(abName, _CurrentSingleABLoader);
                yield return _CurrentSingleABLoader.LoadAssetBundle();
            }
        }

        /// <summary>
        /// 加载引用ab包
        /// </summary>
        /// <param name="abName">AB包名称</param>
        /// <param name="refABName">呗引用ab包名称</param>
        /// <returns></returns>
        private IEnumerator LoadReference(string abName, string refABName)
        {
            if (_DicABRelation.ContainsKey(abName))
            {
                ABRelation tmpABRelationObj = _DicABRelation[abName];
                //添加AB包引用关系(被依赖)
                tmpABRelationObj.AddReference(refABName);
            }
            else
            {
                ABRelation tmpABRelationObj = new ABRelation(abName);
                tmpABRelationObj.AddReference(refABName);
                _DicABRelation.Add(abName,tmpABRelationObj);

                //开始加载依赖的包
                yield return LoadAssetBundle(abName);
            }
        }


        /// <summary>
        /// 加载ab包中的资源
        /// </summary>
        /// <param name="abName">AB名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否使用资源缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string abName, string assetName, bool isCache)
        {
            foreach (string item_abName in _DicSingleABLoaderCache.Keys)
            {
                if (item_abName == abName)
                {
                    return _DicSingleABLoaderCache[item_abName].LoadAsset(assetName, isCache);
                }
            }
            //包应该存在，但是加载不出来。
            Debug.LogError("LoadAsset方法执行失败,abName = " + abName + " assetName = " + assetName);
            return null;
        }

        /// <summary>
        /// 释放本场景中所有的资源    场景转换使用
        /// </summary>
        public void DisPoseAllAsset()
        {
            //逐一释放所有加载过的AB包资源
            try
            {
                foreach (SingleABLoader item_sABLoader in _DicSingleABLoaderCache.Values)
                {
                    item_sABLoader.DisposeAll();
                }
            }
            finally
            {
                _DicSingleABLoaderCache.Clear();
                _DicSingleABLoaderCache = null;
                //释放其他对象占用资源
                _DicABRelation.Clear();
                _DicABRelation = null;
                _CurrentSceneName = null;
                _CurrntABName = null;
                _LoadAllABPackageCompleteHandle = null;
                //卸载未使用的资源
                Resources.UnloadUnusedAssets();
                //强制回收
                System.GC.Collect();
            }
        }
    }
}
