using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFrameWork
{
    /// <summary>
    /// 关系类
    ///     1，存储指定AB包所有依赖关系包
    ///     2，存储指定AB包所有引用关系包
    /// </summary>
    public class ABRelation
    {
        public ABRelation(string abName)
        {
            if (string.IsNullOrEmpty(abName))
            {
                _ABName = abName;
            }
            _ListAllDependenceAB = new List<string>();
            _ListAllReferenceAB = new List<string>();
        }


        //当前AB名称
        private string _ABName;
        //本包所有的依赖包集合
        private List<string> _ListAllDependenceAB;
        //本包所有的引用包集合
        private List<string> _ListAllReferenceAB;

        #region 依赖关系

        /// <summary>
        /// 增加依赖关系
        /// </summary>
        /// <param name="abName"></param>
        public void AddDependence(string abName)
        {
            if (!_ListAllDependenceAB.Contains(abName))
            {
                _ListAllDependenceAB.Add(abName);
            }
        }

        /// <summary>
        ///  移除依赖关系
        /// </summary>
        /// <param name="abName"></param>
        /// true 此AB包没有依赖项
        /// false 此AB包还有依赖项
        public bool RemoveDependence(string abName)
        {
            if (_ListAllDependenceAB.Contains(abName))
            {
                _ListAllDependenceAB.Remove(abName);
               
            }

            if (_ListAllDependenceAB.Count > 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        ///  获取所有依赖关系
        /// </summary>
        public List<string> GetAllDepences()
        {
            return _ListAllDependenceAB;
        }
        #endregion

        #region 引用关系
        /// <summary>
        /// 增加引用关系
        /// </summary>
        public void AddReference(string abName)
        {
            if (!_ListAllReferenceAB.Contains(abName))
            {
                _ListAllReferenceAB.Add(abName);
            }
        }
        /// <summary>
        /// 移除引用关系
        /// </summary>
        /// 
        /// true 此AB包没有引用项
        /// false 此AB包还有引用项
        public bool RemoveReference(string abName)
        {
            if (_ListAllReferenceAB.Contains(abName))
            {
                _ListAllReferenceAB.Remove(abName);
            }
            if (_ListAllReferenceAB.Count > 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取所有引用关系
        /// </summary>
        public List<string> GetAllRefrence()
        {
            return _ListAllReferenceAB;
        }
        #endregion

    }
}
