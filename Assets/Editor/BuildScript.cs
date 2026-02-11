using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    public static void PerformBuild()
    {
        UnityEngine.Debug.Log("=== [CI/CD] Build Process Started ===");

        // 1. 플레이어 설정 (버전 코드 및 키스토어)
        SetupAndroidSettings();

        // 2. 어드레서블 데이터 빌드 (aa 폴더 생성 및 ServerData 업데이트)
        BuildAddressables();

        // 3. AAB 플레이어 빌드 실행
        BuildPlayerOptions buildOptions = GetBuildOptions();
        UnityEngine.Debug.Log("=== [CI/CD] Starting AAB Player Build ===");
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

        // 4. 빌드 결과 체크 및 Firebase 업로드
        if (report.summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log("=== [CI/CD] Player Build Succeeded! Starting Firebase Upload ===");
            UploadToFirebase();
        }
        else
        {
            UnityEngine.Debug.LogError($"=== [CI/CD] Build Failed with result: {report.summary.result} ===");
            EditorApplication.Exit(1); // 젠킨스에 실패 알림
        }

        UnityEngine.Debug.Log("=== [CI/CD] All Processes Finished ===");
    }

    private static void SetupAndroidSettings()
    {
        // 젠킨스 빌드 번호를 가져와 버전 코드 설정
        string buildNumber = System.Environment.GetEnvironmentVariable("BUILD_NUMBER");
        if (!string.IsNullOrEmpty(buildNumber) && int.TryParse(buildNumber, out int versionCode))
        {
            PlayerSettings.Android.bundleVersionCode = versionCode;
            UnityEngine.Debug.Log($"[CI/CD] Set Bundle Version Code: {versionCode}");
        }

        // 키스토어 설정 (보안상 환경변수로 관리하는 것이 좋으나 일단 하드코딩 유지)
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystorePass = "Bajil1016!";
        PlayerSettings.Android.keyaliasName = "tilerunner_upload";
        PlayerSettings.Android.keyaliasPass = "Bajil1016!";

        EditorUserBuildSettings.buildAppBundle = true; // AAB 빌드 강제
        AssetDatabase.SaveAssets();
    }

    private static void BuildAddressables()
    {
        UnityEngine.Debug.Log("=== [CI/CD] Cleaning Addressables Content ===");
        AddressableAssetSettings.CleanPlayerContent(); // 이전 빌드 찌꺼기 제거

        UnityEngine.Debug.Log("=== [CI/CD] Building Addressables Player Content ===");
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

        if (!string.IsNullOrEmpty(result.Error))
        {
            UnityEngine.Debug.LogError($"[CI/CD] Addressables Build Error: {result.Error}");
            EditorApplication.Exit(1);
        }
    }

    private static BuildPlayerOptions GetBuildOptions()
    {
        return new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray(),
            locationPathName = "Builds/Android/TileRunner.aab",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
    }

    private static void UploadToFirebase()
    {
        // 빌드 직후 생성된 ServerData/Android 폴더 경로 (절대경로)
        string serverDataPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "ServerData", "Android"));

        // Firebase Storage 업로드 명령어
        // --non-interactive: 젠킨스에서 입력 대기를 방지합니다.
        // -y: 모든 질문에 Yes
        string command = $"firebase storage:upload \"{serverDataPath}/*\" --non-interactive";

        UnityEngine.Debug.Log($"[CI/CD] Executing Command: {command}");

        ProcessStartInfo proInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(proInfo))
        {
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output)) UnityEngine.Debug.Log($"[Firebase Success]:\n{output}");
            if (!string.IsNullOrEmpty(error)) UnityEngine.Debug.LogError($"[Firebase Error]:\n{error}");
        }
    }
}