using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    void Awake()
    {
        // 최신 버전에서는 별도의 Config 설정 없이 바로 Activate 합니다.
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        Login();
    }

    public void Login()
    {
        // PlayGamesPlatform.Instance를 직접 호출하여 인증을 시도합니다.
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("구글 로그인 성공!");
            // 유저 이름 출력 예시
            Debug.Log("Welcome " + PlayGamesPlatform.Instance.GetUserDisplayName());
        }
        else
        {
            // 상세 에러 상태 확인 (Canceled, InternalError, NetworkError 등)
            Debug.LogWarning("구글 로그인 실패: " + status);
        }
    }
}
