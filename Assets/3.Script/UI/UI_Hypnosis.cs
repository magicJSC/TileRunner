using System;
using System.Collections;
using UnityEngine;

public class UI_Hypnosis : MonoBehaviour, IEndSignal
{
    [SerializeField] int hypnosisCount = 3;
    [SerializeField] float coolTime = 3;
    private int endCount;

    private Animator anim;

    public Action OnEndSignal { get; set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void EndHypnosis()
    {
        endCount++;
        if (endCount >= hypnosisCount)
        {
            SignalEnd();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(PlayHypnosis());
        }
    }

    IEnumerator PlayHypnosis()
    {
         yield return new WaitForSeconds(coolTime);
         anim.Play("Hypnosis", 0, -1);   
    }

    public void AddEndSignalListener(Action listener)
    {
        OnEndSignal += listener;
    }

    public void SignalEnd()
    {
        OnEndSignal?.Invoke();
    }
}
