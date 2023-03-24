#region
using System;
using TMPro;
using UnityEngine;
#endregion

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [ReadOnly, SerializeField] float timeValue = 90f;
    [SerializeField] float startTime = 90f;
    [SerializeField] float maxTimeValue;

    public float TimeValue { get; set; }

    [Header("UI Settings")]
    [SerializeField] TextMeshProUGUI timerText;

    /// <summary>
    /// The bool "countUp" determines whether the timer counts up or down.
    /// </summary>
    [Header("Count Direction")]
    [SerializeField] bool countUp;

    [Header("Private & Cached Variables")]
    bool countDirectionLastFrame;
    PlayerController player;


    void Start()
    {
        countDirectionLastFrame = !countUp;
        timeValue               = startTime;
        player                  = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        Count();
        StopCountdown();
    }

    void Count()
    {
        if (countUp != countDirectionLastFrame)
            //timeValue = countUp ? 0 : maxTimeValue;
            countDirectionLastFrame = countUp;

        timeValue += (countUp ? 1 : -1) * Time.deltaTime;

        timeValue = Mathf.Clamp(timeValue, 0, maxTimeValue);

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0) timeToDisplay = 0;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void AddTime(float timeToAdd) { timeValue += timeToAdd; }

    void StopCountdown()
    {
        if (timeValue <= 0) CD_PlayerDeath();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void CD_PlayerDeath()
    {
        Debug.Log("Player Died!!");
        Destroy(player);

        try
        {
            SceneManagerExtended.ReloadScene();
        } catch (Exception error)
        {
            Debug.LogError("Error: " + error.Message + "\n There is no end-scene scene in the build settings.");
            throw;
        }
    }
}