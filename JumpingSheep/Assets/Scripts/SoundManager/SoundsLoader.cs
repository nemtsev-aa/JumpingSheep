using System;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SoundsLoader {
    private const int Timeout = 5;

    private List<AudioClip> _audioClips;
    private Logger _logger;

    private bool _isAssetsLoaded = false;
    private bool _inProcess = false;

    public SoundsLoader(Logger logger) {
        _logger = logger;

        _isAssetsLoaded = false;
        _inProcess = false;
    }

    public async UniTask<List<AudioClip>> LoadAssets(IReadOnlyList<string> urls) {
        if (_inProcess || _isAssetsLoaded)
            return null;

        _inProcess = true;
        _audioClips = new List<AudioClip>();

        foreach (var url in urls) {
            _audioClips.Add(null);
            await Load(url, _audioClips.Count - 1);
        }

        Reset();

        if (_audioClips[0] == null)
            return null;
        else
            return _audioClips;
    }
    
    public async UniTask LoadAsset(string url, Action<AudioClip> callback) {
        if (_inProcess || _isAssetsLoaded)
            return;

        _inProcess = true;

        var cts = new CancellationTokenSource();
        cts.CancelAfterSlim(TimeSpan.FromSeconds(Timeout));

        try {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            await www.SendWebRequest().WithCancellation(cts.Token);

            _inProcess = false;

            if (www.result == UnityWebRequest.Result.Success) {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                callback?.Invoke(clip);
            }
        }
        catch (OperationCanceledException ex) {

            if (ex.CancellationToken == cts.Token)
                _logger.Log("SoundsLoader: Timeout");
        }
    }

    public async UniTask Load(string url, int id) {
        var cts = new CancellationTokenSource();
        cts.CancelAfterSlim(TimeSpan.FromSeconds(Timeout));

        try {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            await www.SendWebRequest().WithCancellation(cts.Token);

            if (www.result == UnityWebRequest.Result.Success) {
                _audioClips[id] = DownloadHandlerAudioClip.GetContent(www);

                if (IsDone()) {
                    _inProcess = false;
                    _isAssetsLoaded = true;
                }
            }
        }
        catch (OperationCanceledException ex) {

            if (ex.CancellationToken == cts.Token)
                _logger.Log("SoundsLoader: Timeout");

        }
    }

    private bool IsDone() {
        for (int i = 0; i < _audioClips.Count; i++) {
            if (_audioClips[i] == null)
                return false;
        }

        return true;
    }

    private void Reset() {
        _isAssetsLoaded = false;
        _inProcess = false;
    }
}
