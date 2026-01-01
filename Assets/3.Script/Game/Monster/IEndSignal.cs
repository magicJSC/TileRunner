using System;
using UnityEngine;

public interface IEndSignal
{
    public Action OnEndSignal { get; set; }

    public void AddEndSignalListener(Action listener);

    public void SignalEnd();
}
