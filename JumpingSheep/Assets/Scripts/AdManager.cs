using GamePush;
using System;

public class AdManager : IDisposable {
    private const float AdTimeout = 6000f;

    public event Action<string> RewardedReward;
    public event Action<bool> RewardedClosed;
    public event Action<bool> FullscreenClosed;

    private Logger _logger;
    private PauseHandler _pauseHandler;
    private TimeCounter _timeCounter;
    private bool _isTimeout;

    public Platform Platform => GP_Platform.Type();

    public AdManager(Logger logger, PauseHandler pauseHandler, TimeCounter timeCounter) {
        _logger = logger;
        _pauseHandler = pauseHandler;
        _timeCounter = timeCounter;

        _timeCounter.SetTimeValue(AdTimeout);
        TimeoutActivate();

        AddListener();
    }

    public void ShowFullScreen() {
        if (_isTimeout) {
            _logger.Log("ShowFullScreen: Timeout Active");
            return;
        }

        _logger.Log("ShowFullScreen: Timeout Deactive");
        GP_Ads.ShowFullscreen();
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

    private void GP_Ads_OnFullscreenStart() => _pauseHandler.SetPause(true);
    
    private void GP_Ads_OnRewardedStart() => _pauseHandler.SetPause(true);
    
    private void GP_Ads_OnFullscreenClose(bool value) {
        _pauseHandler.SetPause(false);

        TimeoutActivate();
        _logger.Log("Fullscreen Ads Closed");

        FullscreenClosed?.Invoke(value);
    }

    private void GP_Ads_OnRewardedClose(bool value) {
        _pauseHandler.SetPause(false);
        
        RewardedClosed?.Invoke(value);
    } 

    private void GP_Ads_OnRewardedReward(string key) {
        _pauseHandler.SetPause(false);
        RewardedReward?.Invoke(key);
    }

    private void TimeoutActivate() {
        _isTimeout = true;
        _timeCounter.SetWatchStatus(true);

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
