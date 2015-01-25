using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;
using System;

public class BuildScript
{
    public const string StartingLevel = "Assets/UI/Scenes/SplashScreens.unity";

    public static bool HasPro
    {
        get { return UnityEditorInternal.InternalEditorUtility.HasPro(); }
    }

    [MenuItem("Build/Windows")]
    public static void BuildPlayerWindows()
    {
        BuildPlayer(BuildTarget.StandaloneWindows, BuildOptions.None);

        EditorUtility.DisplayDialog("Build Complete", "Build of Windows target is complete!", "OK");
    }

    [MenuItem("Build/MacOSX")]
    public static void BuildPlayerMac()
    {
        BuildPlayer(BuildTarget.StandaloneOSXUniversal, BuildOptions.None);

        EditorUtility.DisplayDialog("Build Complete", "Build of MacOSX target is complete!", "OK");
    }

    [MenuItem("Build/Linux")]
    public static void BuildPlayerLinux()
    {
        BuildPlayer(BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);

        EditorUtility.DisplayDialog("Build Complete", "Build of Linux target is complete!", "OK");
    }

    [MenuItem("Build/All")]
    public static void BuildPlayerAll()
    {
        BuildPlayer(BuildTarget.StandaloneWindows, BuildOptions.None);
        BuildPlayer(BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
        BuildPlayer(BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);

        EditorUtility.DisplayDialog("Build Complete", "Build of all targets is complete!", "OK");
    }

    [MenuItem("GGJ15/Build/Windows", true)]
    [MenuItem("GGJ15/Build/MacOSX", true)]
    [MenuItem("GGJ15/Build/Linux", true)]
    [MenuItem("GGJ15/Build/All", true)]
    public static bool ValidateBuildPlayer()
    {
        return HasPro;
    }

    [PostProcessBuild]
    public static void PostProcessAddXInput(BuildTarget target, string path)
    {
        if (target == BuildTarget.StandaloneWindows)
        {
            File.Copy("XInputInterface.dll", path.Replace("StarshipBravo.exe", "XInputInterface.dll"));
        }
    }

    public static void BuildPlayer(BuildTarget Platform, BuildOptions BuildOptions)
    {
        string path = "Build/";
        string binaryName = "StarshipBravo";

        switch (Platform)
        {
            case BuildTarget.StandaloneWindows:
                path += "Windows/";
                binaryName += ".exe";
                break;
            case BuildTarget.StandaloneOSXUniversal:
                path += "MacOSX/";
                binaryName += ".app";
                break;
            case BuildTarget.StandaloneLinuxUniversal:
                path += "Linux/";
                break;
        }

        List<string> levelFiles = new List<string>();

        levelFiles.Add(StartingLevel);
        foreach (string s in GetFiles("Assets/"))
        {
            if (s.EndsWith(".unity") && !s.Contains(StartingLevel))
            {
                levelFiles.Add(s);
            }
        }

        UpdateScenesFromFiles();
        BuildPipeline.BuildPlayer(levelFiles.ToArray(), path + binaryName, Platform, BuildOptions);
    }

    [MenuItem("Build/Update Scenes From Files")]
    public static void UpdateScenesFromFiles()
    {
        List<string> levelFiles = new List<string>();

        levelFiles.Add(StartingLevel);
        foreach (string s in GetFiles("Assets/"))
        {
            if (s.EndsWith(".unity") && !s.Contains(StartingLevel))
            {
                levelFiles.Add(s);
            }
        }

        EditorBuildSettings.scenes = GetScenesFromFiles(levelFiles.ToArray());
    }

    public static EditorBuildSettingsScene[] GetScenesFromFiles(string[] files)
    {
        List<EditorBuildSettingsScene> list = new List<EditorBuildSettingsScene>();

        foreach (string s in files)
        {
            list.Add(new EditorBuildSettingsScene(s, true));
        }

        return list.ToArray();
    }

    public static IEnumerable<string> GetFiles(string path)
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(path);
        while (queue.Count > 0)
        {
            path = queue.Dequeue();
            try
            {
                foreach (string subDir in Directory.GetDirectories(path))
                {
                    queue.Enqueue(subDir);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            string[] files = null;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    yield return files[i];
                }
            }
        }
    }
}
