using System;

public class MainMenuDialog : Dialog {
    public event Action GameplayDialogShowed;
    public event Action SettingsDialogShowed;
    public event Action AboutDialogShowed;
    public event Action Quited;

    private MenuCategoryPanel _category;

    public override void Init(Logger logger) {
        base.Init(logger);

        logger.Log("Начало метода [DesktopDialog : Init]");
    }

    public override void InitializationPanels() {
        _category = GetPanelByType<MenuCategoryPanel>();
        _category.Init();
    }

    public override void AddListeners() {
        base.AddListeners();

        _category.GameplayDialogSelected += OnGameplayDialogSelected;
        _category.SettingsDialogSelected += OnSettingsDialogSelected;
        _category.AboutDialogSelected += OnAboutDialogSelected;
        _category.QuitButtonSelected += OnQuitButtonSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _category.GameplayDialogSelected -= OnGameplayDialogSelected;
        _category.SettingsDialogSelected -= OnSettingsDialogSelected;
        _category.AboutDialogSelected -= OnAboutDialogSelected;
        _category.QuitButtonSelected -= OnQuitButtonSelected;
    }

    private void OnGameplayDialogSelected() => GameplayDialogShowed?.Invoke();

    private void OnSettingsDialogSelected() => SettingsDialogShowed?.Invoke();

    private void OnAboutDialogSelected() => AboutDialogShowed?.Invoke();

    private void OnQuitButtonSelected() => Quited?.Invoke();

}
