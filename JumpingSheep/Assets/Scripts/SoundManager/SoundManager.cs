using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class SoundManager : MonoBehaviour, IDisposable {
    protected AudioSource AudioSource;

    public abstract void AddListener();

    public abstract void RemoveLisener();

    public virtual void Dispose() {
        RemoveLisener();
    }
}
