using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text cpuDisplay;

    [SerializeField] private TMP_Text ramDisplay;
    // Update is called once per frame
    void FixedUpdate()
    {
        cpuDisplay.text = "CPU Utilization: " + Init.Instance.GetCurrentCPUUsage() + "%";
        ramDisplay.text = "Available Ram: " + Init.Instance.GetAvailableRam() + " MB";
    }
}
