using UnityEditor;

// https://forum.unity.com/threads/build-script-cannot-find-menuitem.535889/#post-3531599
class MyEditorBuild
{
     static void PerformBuild ()
     {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/SampleScene.unity" };
        buildPlayerOptions.locationPathName = "build/linux/app";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.None;
        //buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
