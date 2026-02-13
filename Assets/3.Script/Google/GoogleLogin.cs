using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    void Start()
    {
        // 1. 초기 설정 (필수)
        PlayGamesPlatform.Activate();

        // 2. 로그인 시도
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // 로그인 성공! 
            // 3. 사용자 이름 가져오기 (Social 대신 이 방식을 쓰세요)
            string userName = PlayGamesPlatform.Instance.GetUserDisplayName();
            Debug.Log("환영합니다, " + userName + "님!");
        }
        else
        {
            // 로그인 실패 처리
            Debug.Log("로그인 실패: " + status);
        }
    }
}