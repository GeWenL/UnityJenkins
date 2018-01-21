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
    private static string _version = "2.0";   // 版本号
    public static string PUBLISH_PATH = Application.dataPath + "/../Publish/"; // 发布的资源目录，注意这个是apk的输出路径而不是ab包的输出路径
    


    // 编译程序
    public static void Build() 
    {
        EditorUserBuildSettings.connectProfiler = false;

        // 打包成apk
        var version = _version;
		BuildTarget buildTarget = BuildTarget.iOS;  // 导出平台
		string targetName = "Unity-iPhone";
        BuildTargetGroup buildTargetGroup = BuildTargetGroup.iOS;


        if (!Directory.Exists(PUBLISH_PATH))
        {
            Directory.CreateDirectory(PUBLISH_PATH);
        }

        // 删除之前的包
        if (File.Exists(PUBLISH_PATH + targetName))
        {
            File.Delete(PUBLISH_PATH + targetName);
        }

		PlayerSettings.applicationIdentifier = _bundleIdentifier;
        PlayerSettings.bundleVersion = version;
        PlayerSettings.iOS.buildNumber = 2.1;

        // 当前场景都打包进app里面
        var scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; ++i)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        BuildPlayerOptions options = new BuildPlayerOptions();
        options.locationPathName = Path.GetFullPath(PUBLISH_PATH + targetName); // 注意5.6之后的版本，这个路径里面不能有Assets/这样的字符串，所以要转换为FullPath
        options.scenes = scenes;
        options.target = buildTarget;
        options.targetGroup = buildTargetGroup;
        options.options = BuildOptions.None; //uildOptions.CompressWithLz4;// | BuildOptions.AcceptExternalModificationsToPlayer;


        // 开始构建
		string msg = BuildPipeline.BuildPlayer(options);
        if (!string.IsNullOrEmpty(msg))
        {
            Debug.LogError(msg);
        }

    }


    // 解析命令行
    public static void ParseCommandLine()
    {
        PlayerSettings.iOS.appleEnableAutomaticSigning = false;
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
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                    }
                }
                else if (platform == "ios")
                {
                    if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
                    {
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                    }
                }
            }
            else if (arg == "-teamId")
            {
//#if UNITY_IOS
                if (args[i + 1] != "")
                {
                    // TeamID ios打包使用
                    PlayerSettings.iOS.appleEnableAutomaticSigning = true;
                    PlayerSettings.iOS.appleDeveloperTeamID = args[i + 1];
                }
                
//#endif
            }
        }

    }


}
