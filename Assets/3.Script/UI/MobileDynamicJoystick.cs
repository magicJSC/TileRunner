using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// 인터페이스를 상속받아 이벤트를 처리합니다.
public class MobileDynamicJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;
    [SerializeField] private float dragRange = 100f;

    private Vector2 startPos;

    private Player player;

    void Start()
    {
        if(GameManager.Instance.Player != null)
            GetPlayer();
    }


    // 1. 화면을 누르는 순간 (IPointerDown)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (player == null && GameManager.Instance.Player != null)
            GetPlayer();

        // 터치/클릭한 위치로 조이스틱 이동
        startPos = eventData.position;
        joystickBackground.position = startPos;
        GameManager.Instance.touchSignalAction?.Invoke(true);
    }

    // 2. 드래그 중 (IDrag)
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = eventData.position;
        Vector2 direction = currentPos - startPos;

        float distance = Mathf.Min(direction.magnitude, dragRange);
        Vector2 moveDir = direction.normalized * distance;

        joystickHandle.anchoredPosition = moveDir;

        // 캐릭터 회전 호출 (카메라 방향 보정)
        player.RotateByJoystick(direction.normalized * (distance / dragRange));

    }

    // 3. 손을 뗐을 때 (IPointerUp)
    public void OnPointerUp(PointerEventData eventData)
    {
        joystickBackground.anchoredPosition = new Vector2(0,-642);
        joystickHandle.anchoredPosition = Vector2.zero;
        GameManager.Instance.touchSignalAction?.Invoke(false);
    }

    void GetPlayer()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
    }
}