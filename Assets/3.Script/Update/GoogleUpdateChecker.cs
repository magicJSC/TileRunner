using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
using UnityEngine;

public class GoogleUpdateChecker : MonoBehaviour
{
    AppUpdateManager appUpdateManager;

    void Start()
    {
        appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());
    }

    IEnumerator CheckForUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            // 핵심: 스토어에 새 버전이 있는지 구글이 직접 체크함
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                // 여기서 우리가 만든 업데이트 UI 팝업을 띄우거나, 
                // 구글 공식 업데이트 창을 바로 띄울 수 있습니다.
                StartImmediateUpdate(appUpdateInfoResult);
            }
        }
    }

    void StartImmediateUpdate(AppUpdateInfo info)
    {
        // 구글이 제공하는 '강제 업데이트' 전체 화면 UI를 즉시 실행합니다.
        appUpdateManager.StartUpdate(info, AppUpdateOptions.ImmediateAppUpdateOptions());
    }
}