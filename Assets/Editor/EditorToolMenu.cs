using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;


public class EditorToolMenu : Editor
{
    [MenuItem("Tools/一键打包", false, 2000)]
    static void OnAutoBuild()
    {
        BuildDeploy.ParseCommandLine();
        BuildDeploy.Build();
        EditorUtility.DisplayDialog("信息", "打包完毕!", "确定");
    }

}

