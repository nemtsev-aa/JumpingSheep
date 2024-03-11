using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LevelStatusTypes {
    Locked,
    Ready,
    Complited,
}

public class LevelStatusView : UICompanent {
    [SerializeField] private Image _backgroundImage;
    

    public LevelStatusTypes CurrentStatus { get; private set; }
}
