using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsDisplay;
    private float frames;
    private float runtimeFrames = 200f;
    private float duration;
    private float runtimeDuration = 1f;

    [SerializeField, Range(0.01f, 2f)]
    float sampleDuration = 1f;

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        runtimeFrames += 1;
        runtimeDuration += frameDuration;
        duration += frameDuration;

        if (duration >= sampleDuration)
        {
            fpsDisplay.SetText("FPS\n{0:0}\n000\n000", frames / duration);
            Init.Instance.fps = frames / duration;
            Init.Instance.runtimeFPS = runtimeFrames / runtimeDuration;
            frames = 0;
            duration = 0f;
        }
        Init.Instance.highUsage = frames / duration <= (runtimeFrames / runtimeDuration) * 0.8;
        if (Init.Instance.highUsage)
        {
            print(true);
        }
    }
}
