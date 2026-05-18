using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeState
{
    Normal,
    Slow
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Time Values")]
    public float normalTimeScale = 1f;
    public float slowTimeScale = 0.05f;
    public float transitionSpeed = 8f;

    [Header("Debug")]
    public bool showDebugGUI = true;

    public TimeState CurrentState { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        GameEvents.OnTimeStateRequested += SetTimeState;
    }

    void OnDisable()
    {
        GameEvents.OnTimeStateRequested -= SetTimeState;
    }

    void Start()
    {
        SetTimeState(TimeState.Slow);
        ApplyTimeScaleImmediate(slowTimeScale);
    }

    void Update()
    {
        float targetScale = CurrentState == TimeState.Normal
            ? normalTimeScale
            : slowTimeScale;

        float newScale = Mathf.Lerp(
            Time.timeScale,
            targetScale,
            transitionSpeed * Time.unscaledDeltaTime
        );

        ApplyTimeScale(newScale);
    }

    public void SetTimeState(TimeState newState)
    {
        CurrentState = newState;
    }

    void ApplyTimeScale(float newScale)
    {
        Time.timeScale = Mathf.Clamp(newScale, 0.01f, 1f);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void ApplyTimeScaleImmediate(float newScale)
    {
        Time.timeScale = Mathf.Clamp(newScale, 0.01f, 1f);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void OnGUI()
    {
        if (!showDebugGUI) return;

        GUI.Box(new Rect(10, 10, 260, 115), "Time Debug");
        GUI.Label(new Rect(20, 40, 240, 20), "State: " + CurrentState);
        GUI.Label(new Rect(20, 60, 240, 20), "Current Time Scale: " + Time.timeScale.ToString("F2"));
        GUI.Label(new Rect(20, 80, 240, 20), "Fixed Delta Time: " + Time.fixedDeltaTime.ToString("F4"));
    }
}