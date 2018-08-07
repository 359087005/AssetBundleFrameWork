using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFrameWork
{
    /// <summary>
    /// 工具类
    ///     该框架所有的
    ///     常量
    ///     委托
    ///     枚举
    /// </summary>

    #region     委托
    public delegate void DeLoadComplete(string name);
    #endregion
    public class ABDefine
    {
        public const string AB_MANIFEST = "AssetBundleManifest";
    }
}
