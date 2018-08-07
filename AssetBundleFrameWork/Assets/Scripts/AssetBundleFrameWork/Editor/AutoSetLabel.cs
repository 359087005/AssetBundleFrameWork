using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
/// <summary>
/// 自动为资源文件添加标记
///     1：定义需要打包资源的文件夹根目录
///     2：遍历每个场景文件夹
///     3，遍历本场景目录下所有文件  或目录。
///         如果是目录，则递归访问。
///             如果是文件，则使用AssetIoporter类，标记包名和后缀名。
/// </summary>

namespace AssetBundleFrameWork
{
    public class AutoSetLabel
    {
        /// <summary>
        /// 设置ab包名
        /// </summary>
        [MenuItem("AssetBundleTools/Set AB Label")]
        public static void SetABLabel()
        {
            //需要给ab做标记的根目录
            string strNeedSetlabelRoot = string.Empty;
            //目录信息（场景目录信息数组，所有根目录下的场景目录）
            DirectoryInfo[] dirScenesDirArray = null;
            //清空无用AB标记
            AssetDatabase.RemoveUnusedAssetBundleNames();
            //1：需要打包资源的文件夹根目录
            //strNeedSetlabelRoot = Application.dataPath + "/" + "AB_Res";
            strNeedSetlabelRoot = PathTools.GetABResourcesPath();
            DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedSetlabelRoot);
            dirScenesDirArray = dirTempInfo.GetDirectories();
            //2：遍历每个场景文件夹
            foreach (var currentDir in dirScenesDirArray)
            {
                //如果是目录，则递归访问。
                string tempSceneDir = strNeedSetlabelRoot + "/" + currentDir.Name; //全路径

                int temIndex = tempSceneDir.LastIndexOf("/");
                string tmpScenesName = tempSceneDir.Substring(temIndex + 1); //场景名称
                                                                             //如果是文件，则使用AssetIoporter类，标记包名和后缀名。
                JudgeDIRorFileByRecusive(currentDir, tmpScenesName);
            }
            //刷新信息
            AssetDatabase.Refresh();
            //提示信息 说明打包完成。
            //Debug.Log("打AB包完成");
        }

        /// <summary>
        /// 判断是否为目录与文件
        /// </summary>
        /// <param name="fileSystemInfo">当前文件信息</param>
        /// <param name="scenesName">当前场景的名称</param>
        private static void JudgeDIRorFileByRecusive(FileSystemInfo fileSystemInfo, string scenesName)
        {
            //参数检查
            if (!fileSystemInfo.Exists)
            {
                Debug.LogError("文件或者目录名称:" + fileSystemInfo + "不存在");
                return;
            }
            //得到当前目录下一级文件信息集合
            DirectoryInfo dirInfo = fileSystemInfo as DirectoryInfo; //文件信息转目录信息
            FileSystemInfo[] fileSysArray = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fileInfo in fileSysArray)
            {
                FileInfo fileInfoObj = fileInfo as FileInfo;
                //文件类型
                if (fileInfoObj != null)
                {
                    //修改次文件的AB标签
                    SetFileABLabel(fileInfoObj, scenesName);
                }
                else
                {
                    JudgeDIRorFileByRecusive(fileInfo, scenesName);
                }
            }
        }

        /// <summary>
        /// 给资源设置AB名称以及后缀
        /// </summary>
        /// <param name="fileSystemInfo">(包含文件的绝对路径)</param>
        /// <param name="scenesName"></param>
        private static void SetFileABLabel(FileInfo fileInfoObj, string scenesName)
        {
            //AssetImporter tmpIpmorterObj =  AssetImporter.GetAtPath("文件路径");
            //tmpIpmorterObj.assetBundleName = "AB包名称";
            string strABName = string.Empty;
            string strAssetFilePath = string.Empty;
            //找到文件扩展名
            if (fileInfoObj.Extension == ".meta") return;

            //得到AB包名
            strABName = GetABName(fileInfoObj, scenesName);
            //获取资源的相对路径
            int tempIndex = fileInfoObj.FullName.IndexOf("Assets");
            strAssetFilePath = fileInfoObj.FullName.Substring(tempIndex);
            //为文件的AB名赋值
            AssetImporter tmpImporterObj = AssetImporter.GetAtPath(strAssetFilePath);
            tmpImporterObj.assetBundleName = strABName;

            if (fileInfoObj.Extension == ".unity")
            {
                tmpImporterObj.assetBundleVariant = "u3d";
            }
            else
            {
                tmpImporterObj.assetBundleVariant = "ab";
            }
        }

        /// <summary>
        /// 得到AB包名
        /// </summary>
        /// <param name="fileInfoObj">文件信息</param>
        /// <param name="scenesName">所属场景名称</param>
        /// AB包规则：
        /// AB包名称 = 2级名称+3级名称
        /// <returns></returns>
        private static string GetABName(FileInfo fileInfoObj, string scenesName)
        {
            string strABName = string.Empty;

            //win路径
            string tmpWinPath = fileInfoObj.FullName;
            //替换为unity路径
            string tmpUnityPath = tmpWinPath.Replace(@"\", "/");
            //定位 场景名称
            int tmpSceneNamePos = tmpUnityPath.IndexOf(scenesName) + scenesName.Length; //这里可以获得在整个路径的字符串中 AA/BB/CC/DD/EE/FF    IndexOf获取的是D的路径开始，所以+上D的长度。
            //AB包的类型名称
            string strABFileNameArea = tmpUnityPath.Substring(tmpSceneNamePos + 1); //这里截取是从D的结尾+1开始进行截取。
            if (strABFileNameArea.Contains("/"))
            {
                string[] tempStrArray = strABFileNameArea.Split('/');
                strABName = scenesName + "/" + tempStrArray[0];
            }
            else
            {   //定义*.unity 文件形成特殊AB包名
                strABName = scenesName + "/" + scenesName;
            }
            return strABName;
        }
    }
}