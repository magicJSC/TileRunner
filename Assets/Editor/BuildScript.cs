using UnityEditor;
using System.Linq;

public class BuildScript
{
    public static void PerformBuild()
    {
        // 1. 빌드 폴더 생성
        System.IO.Directory.CreateDirectory("Builds/Android");

        // 2. 버전 코드 자동 증가 (현재 번호에서 +1)
        PlayerSettings.Android.bundleVersionCode += 1;

        // (선택) 버전 이름도 바꾸고 싶다면? 예: "1.0.21" (21은 빌드코드)
        // PlayerSettings.bundleVersion = "1.0." + PlayerSettings.Android.bundleVersionCode;

        // 3. 빌드 타겟 설정 (AAB)
        EditorUserBuildSettings.buildAppBundle = true;

        // 4. 키스토어 비밀번호 설정
        PlayerSettings.Android.keystorePass = "Bajil1016!";
        PlayerSettings.Android.keyaliasPass = "Bajil1016!";

        // 5. 빌드 옵션 설정
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray(),
            locationPathName = "Builds/Android/Project.aab",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // 6. 실행
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}