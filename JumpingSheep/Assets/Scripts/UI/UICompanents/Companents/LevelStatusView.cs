using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum LevelStatusTypes {
    Locked,
    Ready,
    Complited,
}

public class LevelStatusView : UICompanent {
    public event Action<string> Selected;

    [Header("Sprites")]
    [SerializeField] private Sprite _ready;
    [SerializeField] private Sprite _locked;
    [SerializeField] private Sprite _complited;

    [Space(10), Header("Companents")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Transform _starsParent;
    [SerializeField] private Transform _lockParent;
    [SerializeField] private Button _button;

    private LevelStatusViewConfig _config;

    public string Name => _config.Name;
    public LevelStatusTypes Status => _config.CurrentStatus;

    public void Init(LevelStatusViewConfig config) {
        _config = config;

        SetStatus(_config.CurrentStatus);

        AddListeners();
    }

    public void SetStatus(LevelStatusTypes status) {
        if (status == LevelStatusTypes.Locked) {
            ShowLockedStatus();
            return;
        }

        if (status == LevelStatusTypes.Ready) {
            ShowReadyStatus();
            return;
        }

        if (status == LevelStatusTypes.Complited) {
            ShowCompletedStatus();
            return;
        }
    }

    private void ShowReadyStatus() {
        _backgroundImage.sprite = _ready;
        _starsParent.gameObject.SetActive(false);
        _lockParent.gameObject.SetActive(false);
        _nameText.text = _config.Name;

        _config.SetLevelStatus(LevelStatusTypes.Ready);
    }

    private void ShowLockedStatus() {
        _backgroundImage.sprite = _locked;
        _starsParent.gameObject.SetActive(false);
        _lockParent.gameObject.SetActive(true);
        _nameText.text = "";

        _config.SetLevelStatus(LevelStatusTypes.Locked);
    }

    private void ShowCompletedStatus() {
        _backgroundImage.sprite = _complited;

        ShowStars();

        _lockParent.gameObject.SetActive(false);
        _nameText.text = _config.Name;

        _config.SetLevelStatus(LevelStatusTypes.Complited);
    }

    private void ShowStars() {
        _starsParent.gameObject.SetActive(true);

        for (int i = 0; i == _config.StarsCount; i++) {
            _starsParent.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void AddListeners() {
        _button.onClick.AddListener(ButtonClick);
    }

    private void RemoveListeners() {
        _button.onClick.RemoveListener(ButtonClick);
    }

    private void ButtonClick() {
        if (_config.CurrentStatus == LevelStatusTypes.Locked)
            return;

        Selected?.Invoke(_config.Name);
    }

    public override void Dispose() {
        base.Dispose();

        RemoveListeners();
    }
}
