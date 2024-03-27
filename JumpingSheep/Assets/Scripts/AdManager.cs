using GamePush;
using System;

public class AdManager : IDisposable {
    public event Action<string> RewardedReward;
    public event Action<bool> RewardedClosed;
    public event Action<bool> FullscreenClosed;

    private PauseHandler _pauseHandler;

    public Platform Platform => GP_Platform.Type();

    public AdManager(PauseHandler pauseHandler) {
        _pauseHandler = pauseHandler;

        AddListener();
    }

    public void ShowFullScreen() => GP_Ads.ShowFullscreen();

    public void ShowRevarded() => GP_Ads.ShowRewarded();

    private void AddListener() {
        GP_Ads.OnFullscreenStart += GP_Ads_OnFullscreenStart;
        GP_Ads.OnFullscreenClose += GP_Ads_OnFullscreenClose;

        GP_Ads.OnRewardedStart += GP_Ads_OnRewardedStart;
        GP_Ads.OnRewardedClose += GP_Ads_OnRewardedClose;
        GP_Ads.OnRewardedReward += GP_Ads_OnRewardedReward;
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

    public void Dispose() {
        RemoveListener();
    }
}
