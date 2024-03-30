using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class SoundManager : MonoBehaviour, IDisposable {
    [SerializeField] protected VolumeConfig Volume;

    protected AudioSource AudioSource;
    protected bool IsInit;

    public abstract void AddListener();

    public abstract void RemoveLisener();

    public virtual void Dispose() {
        RemoveLisener();
    }
}
