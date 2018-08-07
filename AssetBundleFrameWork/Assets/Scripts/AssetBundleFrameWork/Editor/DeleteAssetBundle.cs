using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
/// <summary>
/// 删除AB包
/// </summary>
namespace AssetBundleFrameWork
{
    public class DeleteAssetBundle
    {
        [MenuItem("AssetBundleTools/DeleteAllAB")]
        public static void DeleteAllAssetBundle()
        {
            //删除AB包输出目录
            string strNeedDeleteDir = string.Empty;

            strNeedDeleteDir = PathTools.GetABOutPath();
            if (string.IsNullOrEmpty(strNeedDeleteDir))
            {
                //参数true表示可以删除非空目录
                Directory.Delete(strNeedDeleteDir,true);
                //去除删除警告
                File.Delete(strNeedDeleteDir + ".meta");
                AssetDatabase.Refresh();
            }
        }
    }
}

