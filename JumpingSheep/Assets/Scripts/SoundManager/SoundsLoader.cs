using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SoundsLoader : MonoBehaviour {
    private List<AudioClip> _audioClips;
    private Logger _logger;

    private IEnumerator _coroutine;

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

            if (IsDone()) {
                _inProcess = false;
                _isAssetsLoaded = true;
            }
        }
        else
            _logger.Log(www.error);
    }

    public List<AudioClip> LoadingClips(IReadOnlyList<string> urls) {
        if (_inProcess || _isAssetsLoaded)
            return null;

        _inProcess = true;
        _audioClips = new List<AudioClip>();

        foreach (var url in urls) {
            _audioClips.Add(null);

            _coroutine = LoadingClip(url, _audioClips.Count - 1);

            StartCoroutine(_coroutine);
        }

        Reset();

        if (_audioClips[0] == null)
            return null;
        else
            return _audioClips;
    }

    private IEnumerator LoadingClip(string url, int id) {
        _isAssetsLoaded = true;

        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                _audioClips[id] = DownloadHandlerAudioClip.GetContent(request);

                if (IsDone()) {
                    _inProcess = false;
                    _isAssetsLoaded = true;
                }
            }
            else
                _logger.Log(request.error);
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
