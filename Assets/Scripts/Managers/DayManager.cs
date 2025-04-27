using UnityEngine;

public enum Day
{
    Day1,
    Day2,
    Day3,
    Day4,
    Day5,
}

public class DayManager : MonoBehaviour
{
    public Day currentDay = Day.Day1;

    public void NextDay()
    {
        if (currentDay < Day.Day5)
        {
            currentDay++;
            Debug.Log("Day Changed: Current Day: " + currentDay);
        }
        else
        {
            Debug.Log("Last day reached. No more days to increment.");
        }
    }

    public bool IsDay(Day day)
    {
        return currentDay == day;
    }
}
