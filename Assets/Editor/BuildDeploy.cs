//#define UNITY_IOS

using UnityEngine;  
using UnityEditor;  
using System.Collections;  
using System.Collections.Generic;  
using System;  
using System.IO; 
using System.Text;
#if UNITY_IOS
using UnityEditor.iOS.Xcode; 
#endif
using Debug = UnityEngine.Debug;



public class BuildDeploy : Editor
{
    private static string _bundleIdentifier = "com.xx.xxx";  // 打包的签名
    private static string _version = "1.0";   // 版本号
    public static string PUBLISH_PATH = Application.dataPath + "/../Publish/"; // 发布的资源目录，注意这个是apk的输出路径而不是ab包的输出路径
    


    // 编译程序
    public static void Build() 
    {
        EditorUserBuildSettings.connectProfiler = false;

        // 打包成apk
        var version = _version;
		BuildTarget buildTarget = BuildTarget.iOS;  // 导出平台
		string targetName = "Unity-iPhone";
        if (!Directory.Exists(PUBLISH_PATH))
        {
            Directory.CreateDirectory(PUBLISH_PATH);
        }

        // 删除之前的包
        if (File.Exists(PUBLISH_PATH + targetName))
        {
            File.Delete(PUBLISH_PATH + targetName);
        }

		PlayerSettings.bundleIdentifier = _bundleIdentifier;
        PlayerSettings.bundleVersion = version;


        // 当前场景都打包进app里面
        var scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; ++i)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        // 开始构建
        int startTime = Environment.TickCount;
		string msg = BuildPipeline.BuildPlayer(scenes, Path.GetFullPath(PUBLISH_PATH + targetName),buildTarget, BuildOptions.None);
        Debug.Log("打包耗时: " + (Environment.TickCount - startTime));
        if (!string.IsNullOrEmpty(msg))
        {
            Debug.LogError(msg);
        }

    }


    // 解析命令行
    public static void ParseCommandLine()
    {
        // 设置当前的id
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length <= 0) return;

        for (int i = 0; i < args.Length - 1; ++i)
        {
            string arg = args[i];
            if (arg == "-bundleId")
            {
                // 包唯一id
                _bundleIdentifier = args[i + 1];
            }
            else if (arg == "-platform")
            {
                // 切换平台
                string platform = args[i + 1];
                if (platform == "android")
                {
                    if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                    {
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
                    }
                }
                else if (platform == "ios")
                {
                    if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
                    {
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
                    }
                }
            }
            else if (arg == "-teamId")
            {
//#if UNITY_IOS
                // TeamID ios打包使用
//                PlayerSettings.iOS.appleDeveloperTeamID = args[i + 1];
//#endif
            }
        }

    }


}
