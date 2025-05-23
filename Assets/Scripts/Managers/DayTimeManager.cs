using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }

    public Light directionalLight;
    [SerializeField] private TimeOfDay currentTimeOfDay;

    void Start()
    {
        UpdateDirectionalLight();
    }

    public void SetTimeOfDay(TimeOfDay newTimeOfDay)
    {
        currentTimeOfDay = newTimeOfDay;
        UpdateDirectionalLight();
    }

    public bool IsDayTime(TimeOfDay timeOfDay)
    {
        return currentTimeOfDay == timeOfDay;
    }

    private void UpdateDirectionalLight()
    {
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Morning:
                directionalLight.color = Color.yellow;
                directionalLight.transform.rotation = Quaternion.Euler(50, -30, 0);
                break;
            case TimeOfDay.Afternoon:
                directionalLight.color = Color.white;
                directionalLight.transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case TimeOfDay.Evening:
                directionalLight.color = new Color(1f, 0.5f, 0.5f);
                directionalLight.transform.rotation = Quaternion.Euler(120, 30, 0);
                break;
            case TimeOfDay.Night:
                directionalLight.color = new Color(0.1f, 0.1f, 0.35f);
                directionalLight.transform.rotation = Quaternion.Euler(195, 0, 0);
                break;
        }
    }
}