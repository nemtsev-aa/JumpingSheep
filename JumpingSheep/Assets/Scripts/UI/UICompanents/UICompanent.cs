using UnityEngine;
using System;

[Serializable]
public class UICompanent : MonoBehaviour, IDisposable {
    public virtual void Show(bool value) {
        gameObject.SetActive(value);
    }

    public virtual void Dispose() {
        
    }
}
