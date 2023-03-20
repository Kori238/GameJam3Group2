using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsDisplay;
    private float frames;
    private float duration;

    [SerializeField, Range(0.01f, 2f)]
    float sampleDuration = 1f;

    // Update is called once per frame
    void Update () {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        duration += frameDuration;

        if (duration >= sampleDuration) {
            fpsDisplay.SetText("FPS\n{0:0}\n000\n000", frames / duration);
            frames = 0;
            duration = 0f;
        }
    }
}
