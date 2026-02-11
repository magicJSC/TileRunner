using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets; // 추가 필요
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class BuildScript
{
    public static void PerformBuild()
    {
        Debug.Log("=== BuildScript.PerformBuild Started ===");

        // 1. 버전 및 환경 설정 (빌드 시작 전에 먼저 처리)
        string buildNumber = System.Environment.GetEnvironmentVariable("BUILD_NUMBER");
        int versionCode;

        if (!string.IsNullOrEmpty(buildNumber) && int.TryParse(buildNumber, out int parsed))
        {
            versionCode = parsed;
            Debug.Log($"[CI] Using BUILD_NUMBER: {versionCode}");
        }
        else
        {
            versionCode = PlayerSettings.Android.bundleVersionCode + 1;
            Debug.Log($"[CI] Incrementing local version: {versionCode}");
        }

        PlayerSettings.Android.bundleVersionCode = versionCode;

        // 키스토어 설정
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystorePass = "Bajil1016!";
        PlayerSettings.Android.keyaliasName = "tilerunner_upload";
        PlayerSettings.Android.keyaliasPass = "Bajil1016!";

        AssetDatabase.SaveAssets();

        // 2. 어드레서블 데이터 빌드 (플레이어 빌드 전에 수행되어야 aa 폴더가 포함됨)
        Debug.Log("Addressables 빌드 시작...");
        AddressableAssetSettings.BuildPlayerContent();

        // 3. 빌드 준비
        System.IO.Directory.CreateDirectory("Builds/Android");
        EditorUserBuildSettings.buildAppBundle = true; // AAB 생성 설정

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

        // 4. 최종 플레이어 빌드 (딱 한 번만 호출)
        Debug.Log("AAB 빌드 시작...");
        var report = BuildPipeline.BuildPlayer(options);

        Debug.Log($"=== Build Finished. Result: {report.summary.result} ===");
    }
}