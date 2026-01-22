using Unity.Cinemachine;
using UnityEngine;

public class GameCam : MonoBehaviour
{
    private CinemachineCamera cam;

    private void Start()
    {
        cam = GetComponent<CinemachineCamera>();

        GameManager.Instance.playerAction += SetPlayer;
        SetPlayer();
    } 

    void SetPlayer()
    {
        if (GameManager.Instance.Player == null)
            return;

        cam.Target.TrackingTarget = GameManager.Instance.Player;
    }
}
