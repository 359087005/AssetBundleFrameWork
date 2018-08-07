using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AssetBundleFrameWork
{
    /// <summary>
    /// 打包
    /// </summary>
    public class BuildAssetsBundle
    {
        [MenuItem("AssetBundleTools/BuildAllAssetBundles")]
        public static void BuildAllAB()
        {
            //打包 输出路径
            string strBuildABPathDir = string.Empty;
            //获取streamingAssets
            strBuildABPathDir = PathTools.GetABOutPath();
                                                                //如果路径不存在
            if (!Directory.Exists(strBuildABPathDir))
            {
                Directory.CreateDirectory(strBuildABPathDir);
            }
            BuildPipeline.BuildAssetBundles(strBuildABPathDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
    }
}

