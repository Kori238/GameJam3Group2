using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsDisplay;
    private float _frames;
    private float _runtimeFrames = 1200f;
    private float _duration;
    private float _runtimeDuration = 1f;

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        _frames += 1;
        _runtimeFrames += 1;
        _runtimeDuration += frameDuration;
        _duration += frameDuration;

        if (_duration >= Time.deltaTime)
        {
            
            Init.Instance.fps = _frames / _duration;
            Init.Instance.runtimeFPS = _runtimeFrames / _runtimeDuration;
            fpsDisplay.SetText("FPS\n{0:0}\n{1:0}\n000", Init.Instance.fps, Init.Instance.runtimeFPS);
            _frames = 0;
            _duration = 0f;
        }
        Init.Instance.highUsage = _frames / _duration <= (_runtimeFrames / _runtimeDuration);
        if (Init.Instance.highUsage)
        {
            print(true);
        }
    }
}
