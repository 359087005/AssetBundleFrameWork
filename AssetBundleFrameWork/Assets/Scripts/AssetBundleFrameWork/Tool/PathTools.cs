using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含本框架中所有的路径常量
/// </summary>

namespace AssetsBundleFrameWork
{
    public class PathTools
    {
        //路径常量
        public const string AB_RESOURCES = "AB_Res";


        /// <summary>
        /// ab资源的输入路径
        /// </summary>
        /// <returns></returns>
        public static string GetABResourcesPath()
        {
            return Application.dataPath + "/" + AB_RESOURCES;
        }

        /// <summary>
        /// 获得ab输出路径
        /// 根据平台获得
        /// </summary>
        public static string GetABOutPath()
        {
            return GetPlatformPath() + "/"+ GetPlatformName();
        }
        /// <summary>
        /// 获取平台路径
        /// </summary>
        /// <returns></returns>
        private static string GetPlatformPath()
        {
            string strReturnPlatformPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformPath = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    strReturnPlatformPath = Application.persistentDataPath;
                    break;
            }
            return strReturnPlatformPath;
        }
        /// <summary>
        /// 获取平台的名称
        /// </summary>
        /// <returns></returns>
        private static string GetPlatformName()
        {
            string strReturnPlatformName = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformName = "Windows";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    strReturnPlatformName = "IPhone";
                    break;
                case RuntimePlatform.Android:
                    strReturnPlatformName = "Android";
                    break;
            }
            return strReturnPlatformName;
        }
    }
}
