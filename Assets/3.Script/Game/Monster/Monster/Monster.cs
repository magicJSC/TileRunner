using DG.Tweening;
using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] protected float declineAmount = 15;
    [SerializeField] protected Vector3 spawnPos;
    protected Action<int,int> dieAction;

    protected int spawnIndex;
    protected int spawnLevel;

    public void Init(int index, int spawnLevel, Action<int,int> dieAction)
    {
        spawnIndex = index;
        this.spawnLevel = spawnLevel;
        this.dieAction += dieAction;
    }

    public virtual void Die()
    {
        dieAction?.Invoke(spawnIndex, spawnLevel);
        transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => Destroy(gameObject));
        BossController.Instance.DecreaseGage(declineAmount);
    }
}
