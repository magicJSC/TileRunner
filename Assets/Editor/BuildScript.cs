using UnityEditor;
using System.Linq;
using UnityEngine;

public class BuildScript
{
    public static void PerformBuild()
    {
        Debug.Log("=== BuildScript.PerformBuild Started ===");

        // Jenkins 환경 변수 확인
        string buildNumber = System.Environment.GetEnvironmentVariable("BUILD_NUMBER");
        int versionCode;

        if (!string.IsNullOrEmpty(buildNumber) && int.TryParse(buildNumber, out int parsed))
        {
            versionCode = parsed;
            Debug.Log($"[CI] Using BUILD_NUMBER from Jenkins: {versionCode}");
        }
        else
        {
            versionCode = PlayerSettings.Android.bundleVersionCode + 1;
            Debug.Log($"[CI] BUILD_NUMBER not found. Incrementing local version: {versionCode}");
        }

        // 버전 코드 강제 설정 및 저장
        PlayerSettings.Android.bundleVersionCode = versionCode;
        AssetDatabase.SaveAssets(); // 설정값을 디스크에 강제 저장

        Debug.Log($"[CI] Final PlayerSettings VersionCode: {PlayerSettings.Android.bundleVersionCode}");

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystorePass = "Bajil1016!"; // 키스토어 비번
        PlayerSettings.Android.keyaliasName = "tilerunner_upload";         // 키 에일리어스 이름
        PlayerSettings.Android.keyaliasPass = "Bajil1016!";

        // 빌드 폴더 생성
        System.IO.Directory.CreateDirectory("Builds/Android");

        EditorUserBuildSettings.buildAppBundle = true;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray(),
            locationPathName = "Builds/Android/TileRunner.aab",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);
        Debug.Log($"=== Build Finished. Result: {report.summary.result} ===");
    }
}