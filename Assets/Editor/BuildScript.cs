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
        // 1. 설정 파일 로드 확인
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            UnityEngine.Debug.LogError("[CI/CD] Addressable Settings를 찾지 못했습니다.");
            return;
        }

        // 2. [중요] 이전 빌드 데이터 클린업
        UnityEngine.Debug.Log("=== [CI/CD] Cleaning Old Addressables Data ===");
        AddressableAssetSettings.CleanPlayerContent();

        // 3. 빌드 실행
        // BuildScript.cs 수정 예시
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        if (!string.IsNullOrEmpty(result.Error))
        {
            // 단순 SBP Error 외에 구체적인 이유가 담겨있는지 확인
            UnityEngine.Debug.LogError($"[CI/CD] 구체적 에러 내용: {result.Error}");
            throw new Exception("Addressables Build Failed: " + result.Error);
        }

        // 4. 에러가 있다면 상세 내용 출력 후 중단
        if (!string.IsNullOrEmpty(result.Error))
        {
            UnityEngine.Debug.LogError($"[CI/CD] Addressables Build 실패: {result.Error}");
            // 강제 종료하지 않고 Exception을 던져서 로그를 남기게 함
            throw new System.Exception("Addressables Build Failed: " + result.Error);
        }

        UnityEngine.Debug.Log("=== [CI/CD] Addressables Build SUCCESS ===");
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
        // 1. 경로 정의
        string serverDataPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "ServerData", "Android"));
        string publicAndroidPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "ServerData", "Android"));

        // ServerData가 없으면 빌드 실패로 간주
        if (!Directory.Exists(serverDataPath))
        {
            UnityEngine.Debug.LogError($"[CI/CD] ServerData 경로가 없습니다: {serverDataPath}");
            return; // 여기서 Exception을 던지면 PerformBuild가 중단됩니다.
        }

        try
        {
            // 2. [강력 삭제] Android 폴더가 있으면 하위 파일까지 싹 지움
            if (Directory.Exists(publicAndroidPath))
            {
                UnityEngine.Debug.Log($"[CI/CD] Deleting old files at: {publicAndroidPath}");
                // 폴더 안의 파일들을 하나씩 지우거나 폴더 자체를 삭제
                Directory.Delete(publicAndroidPath, true);
            }

            // 3. 다시 깨끗하게 생성
            Directory.CreateDirectory(publicAndroidPath);

            // 4. 최신 파일만 복사
            string[] files = Directory.GetFiles(serverDataPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(publicAndroidPath, fileName);
                File.Copy(file, destFile);
                UnityEngine.Debug.Log($"[CI/CD] Copied: {fileName}");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"[CI/CD] File cleanup failed: {e.Message}");
        }
        // 2. 평소 쓰시던 deploy 명령 실행
        string command = "firebase deploy --only hosting";

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