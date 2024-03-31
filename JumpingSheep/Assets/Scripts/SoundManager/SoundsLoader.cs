using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections.Generic;

public class SoundsLoader {
    private List<AudioClip> _audioClips;
    private Logger _logger;
    private bool _isAssetsLoaded = false;
    private bool _inProcess = false;

    public void Init(Logger logger) {
        _logger = logger;

        _isAssetsLoaded = false;
        _inProcess = false;
    }

    public async Task<List<AudioClip>> LoadAssets(IReadOnlyList<string> urls) {
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

    private async Task Load(string url, int id) {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        www.SendWebRequest();

        while (!www.isDone) {
            await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success) {
            _audioClips[id] = DownloadHandlerAudioClip.GetContent(www);
            _logger.Log($"AudioClip loaded: {_audioClips[id].name}");

            if (IsDone()) {
                _inProcess = false;
                _isAssetsLoaded = true;
            }
        }
        else
            _logger.Log(www.error);
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
