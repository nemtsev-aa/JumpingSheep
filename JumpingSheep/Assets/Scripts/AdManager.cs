using GamePush;
using System;

public class AdManager : IDisposable {
    private const float AdTimeout = 60f;

    public event Action FullscreenStarted;
    public event Action<bool> FullscreenClosed;

    public event Action<string> RewardedReward;
    public event Action RewardedStarted;
    public event Action<bool> RewardedClosed;

    private Logger _logger;
    private TimeCounter _timeCounter;
    private bool _isTimeout;

    public Platform Platform => GP_Platform.Type();

    public AdManager(Logger logger, TimeCounter timeCounter) {
        _logger = logger;
        _timeCounter = timeCounter;

        _timeCounter.SetTimeValue(AdTimeout);
        TimeoutActivate();

        AddListener();
    }

    public bool TryShowFullScreen() {
        //if (Platform != GamePush.Platform.YANDEX)
        //    return false;

        if (_isTimeout) {
            _logger.Log($"ShowFullScreen: Timeout Active - {_timeCounter.RemainingTime}");
            return false;
        }

        _logger.Log("ShowFullScreen: Active showed");

        GP_Ads.ShowFullscreen();
        return true;
    } 

    public void ShowRevarded() => GP_Ads.ShowRewarded();

    private void AddListener() {
        GP_Ads.OnFullscreenStart += GP_Ads_OnFullscreenStart;
        GP_Ads.OnFullscreenClose += GP_Ads_OnFullscreenClose;

        GP_Ads.OnRewardedStart += GP_Ads_OnRewardedStart;
        GP_Ads.OnRewardedClose += GP_Ads_OnRewardedClose;
        GP_Ads.OnRewardedReward += GP_Ads_OnRewardedReward;

        _timeCounter.TimeIsOver += OnTimeoutFinished;
    }

    private void RemoveListener() {
        GP_Ads.OnFullscreenStart -= GP_Ads_OnFullscreenStart;
        GP_Ads.OnFullscreenClose -= GP_Ads_OnFullscreenClose;

        GP_Ads.OnRewardedStart -= GP_Ads_OnRewardedStart;
        GP_Ads.OnRewardedClose -= GP_Ads_OnRewardedClose;
        GP_Ads.OnRewardedReward -= GP_Ads_OnRewardedReward;
    }

    private void GP_Ads_OnFullscreenStart() => FullscreenStarted?.Invoke();
    
    private void GP_Ads_OnRewardedStart() => RewardedStarted?.Invoke();

    private void GP_Ads_OnFullscreenClose(bool value) {
        TimeoutActivate();
        _logger.Log("Fullscreen Ads Closed");

        FullscreenClosed?.Invoke(value);
    }

    private void GP_Ads_OnRewardedClose(bool value) => RewardedClosed?.Invoke(value);
    
    private void GP_Ads_OnRewardedReward(string key) => RewardedReward?.Invoke(key);
    
    private void TimeoutActivate() {
        _isTimeout = true;
        _timeCounter.SetTimerStatus(true);

        _logger.Log("Timeout Activated");
    }

    private void OnTimeoutFinished() {
        _isTimeout = false;
        _logger.Log("Timeout Finished");
    }
    
    public void Dispose() {
        RemoveListener();
    }
}
