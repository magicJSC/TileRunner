using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreSwipe : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Camera cam;
    private Transform[] characterPoints;
    public float moveDuration = 0.3f;

    int currentIndex;
    Vector2 dragStartPos;
    Vector3 camStartPos;
    bool dragging;

    float distancePerChar;

    public Action<int> swipeAction;

    void Start()
    {
        currentIndex = CharacterManger.Instance.useIndex;
        cam = CharacterManger.Instance.storeCam;
        characterPoints = CharacterManger.Instance.characterPoints;

        cam.transform.position = GetCamPos(CharacterManger.Instance.useIndex);
        distancePerChar = characterPoints[1].position.x - characterPoints[0].position.x;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        dragStartPos = eventData.position;
        camStartPos = cam.transform.position;
        cam.transform.DOKill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging) return;

        float deltaX = eventData.position.x - dragStartPos.x;
        float worldMove = -deltaX / Screen.width * distancePerChar;

        cam.transform.position = camStartPos + Vector3.right * worldMove;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!dragging) return;
        dragging = false;

        float moved = cam.transform.position.x - camStartPos.x;
        float ratio = Mathf.Abs(moved) / distancePerChar;

        int dir = 0;
        if (ratio >= 0.35f)
            dir = moved > 0 ? 1 : -1;

        SnapToIndex(currentIndex + dir);
    }

    void SnapToIndex(int index)
    {
        index = Mathf.Clamp(index, 0, characterPoints.Length - 1);
        currentIndex = index;

        cam.transform.DOMove(GetCamPos(index), moveDuration)
            .SetEase(Ease.OutCubic);

        swipeAction?.Invoke(currentIndex);
    }

    Vector3 GetCamPos(int index)
    {
        Vector3 pos = cam.transform.position;
        pos.x = characterPoints[index].position.x;
        return pos;
    }
}
