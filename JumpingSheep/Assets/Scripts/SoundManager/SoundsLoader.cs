using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundsLoader : MonoBehaviour {

    private List<AudioClip> _audioClips;

    private bool _isAssetsLoaded = false;
    private bool _inProcess = false;

    //private MonoBehaviour _context;

    public SoundsLoader() {
        _isAssetsLoaded = false;
        _inProcess = false;
    }

    public void Init() {
        _isAssetsLoaded = false;
        _inProcess = false;
    }

    public List<AudioClip> LoadAssets(IReadOnlyList<string> urls) {
        if (_inProcess)
            return null;

        if (_isAssetsLoaded)
            return null;

        _inProcess = true;
        //_context = context;
        _audioClips = new List<AudioClip>();

        foreach (var url in urls) {
            _audioClips.Add(null);
            StartCoroutine(LoadClip(url, _audioClips.Count - 1));
        }

        if (_audioClips[0] == null) 
            return null;
        else
            return _audioClips;

    }
    
    private IEnumerator LoadClip(string url, int id) {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG)) {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                _audioClips[id] = DownloadHandlerAudioClip.GetContent(www);
                
                if (IsDone()) {
                    _inProcess = false;
                    _isAssetsLoaded = true;
                }
            }
            else 
            {
                Debug.Log(www.error);
            }
        }
    }

    private bool IsDone() {
        for (int i = 0; i < _audioClips.Count; i++) {
            if (_audioClips[i] == null)
                return false;
        }

        return true;
    }
}
