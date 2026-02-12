using System.Collections;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;

public class UpdateController : MonoBehaviour
{
    private AppUpdateManager appUpdateManager;

    private void Start()
    {
        // 에디터 환경이 아닌 실제 안드로이드 기기에서만 작동합니다.
        #if !UNITY_EDITOR && UNITY_ANDROID
        StartCoroutine(CheckForUpdate());
        #else
        Debug.Log("에디터 환경에서는 업데이트 체크를 건너뜁니다.");
        #endif
    }

    private IEnumerator CheckForUpdate()
    {
        appUpdateManager = new AppUpdateManager();

        // 1. 업데이트 정보 가져오기 요청
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            // 2. 업데이트가 사용 가능한지 확인 (UpdateAvailable)
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                Debug.Log("새로운 업데이트가 발견되었습니다. 즉시 업데이트를 시작합니다.");

                // 3. 즉시 업데이트(Immediate) 시작
                // 유저가 업데이트를 완료할 때까지 앱 제어권이 구글 플레이로 넘어갑니다.
                var startUpdateOperation = appUpdateManager.StartUpdate(
                    appUpdateInfoResult,
                    AppUpdateOptions.ImmediateAppUpdateOptions());

                yield return startUpdateOperation;
            }
            else
            {
                Debug.Log("현재 최신 버전을 사용 중입니다.");
            }
        }
        else
        {
            Debug.LogError($"업데이트 체크 실패: {appUpdateInfoOperation.Error}");
        }
    }

    // 앱이 다시 포커스를 얻었을 때(업데이트 중 취소 등)를 대비한 로직
    private void OnEnable()
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
        StartCoroutine(ResumeUpdate());
        #endif
    }

    private IEnumerator ResumeUpdate()
    {
        if (appUpdateManager == null) yield break;

        var appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var result = appUpdateInfoOperation.GetResult();
            // 이미 업데이트가 진행 중인 경우(DeveloperTriggeredUpdateInProgress) 다시 화면을 띄움
            if (result.UpdateAvailability == UpdateAvailability.DeveloperTriggeredUpdateInProgress)
            {
                appUpdateManager.StartUpdate(result, AppUpdateOptions.ImmediateAppUpdateOptions());
            }
        }
    }
}