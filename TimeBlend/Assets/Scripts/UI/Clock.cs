using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public (int Hour, int Minute, int Second) startTime = (13, 28, 30);
    public TMP_Text clockText;
    public float timeScale = 2.0f; // ✅ Fixed missing semicolon
    private float timer;

    private int currentHour;
    private int currentMinute;
    private int currentSecond;

    void Start()
    {
        currentHour = startTime.Hour;
        currentMinute = startTime.Minute;
        currentSecond = startTime.Second;
    }

    void Update()
    {
        timer += Time.deltaTime * timeScale;

        if (timer >= 1f)
        {
            timer = 0f;
            updateTime();
            showTime();
        }
    }

    void updateTime()
    {
        currentSecond++;

        if (currentSecond >= 60)
        {
            currentSecond = 0;
            currentMinute++;
        }
        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour = (currentHour + 1) % 24;
        }
    }

    void showTime()
    {
        if (clockText != null)
        {
            clockText.text = $"{currentHour:D2}:{currentMinute:D2}:{currentSecond:D2}";
        }
    }
}
