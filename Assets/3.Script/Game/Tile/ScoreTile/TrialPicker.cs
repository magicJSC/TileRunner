using System.Collections.Generic;
using UnityEngine;

public class TrialPicker : MonoBehaviour
{
    public static TrialPicker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<TrialSO> trialList;

    public TrialSO PickTrial()
    {
       int idx = Random.Range(0, trialList.Count);
       return trialList[idx];
    }
}
