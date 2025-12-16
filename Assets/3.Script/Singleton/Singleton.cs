using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // 이미 존재하면 바로 반환
            if (_instance != null)
                return _instance;

            // 1) 씬에서 먼저 탐색 (기존 오브젝트 보호)
            _instance = FindAnyObjectByType<T>();
            if (_instance != null)
                return _instance;

            // 2) 없으면 동적으로 생성
            GameObject go = new GameObject(typeof(T).Name);
            _instance = go.AddComponent<T>();

            DontDestroyOnLoad(go);
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 이미 Instance가 있다면 → 이 객체가 새로 생긴 객체
        // 새 객체는 삭제(원본 보호)
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 첫 번째 생성된 싱글톤만 살아남음
        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
