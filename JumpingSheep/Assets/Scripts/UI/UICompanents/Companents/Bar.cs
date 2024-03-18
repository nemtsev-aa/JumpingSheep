using UnityEngine;
using UnityEngine.UI;

public abstract class Bar : UIPanel {
    [SerializeField] protected Image Filler;

    protected abstract void OnValueChanged(float currentValue, float maxValue);
}
