using System;
using UnityEngine;

public class DateManager : Singleton<DateManager>
{
    [SerializeField] private int _firstMonth;
    [SerializeField] private int _firstDay;

    private Date _currentDate;

    void Awake()
    {
        if (CheckInstance())
        {
            initialize();
        }
    }

    private void initialize()
    {
        _currentDate.Month = _firstMonth;
        _currentDate.Day = _firstDay;
    }

    public Date GetCurrentDate()
    {
        return _currentDate;
    }

    public void SetCurrentDate(Date date)
    {
        _currentDate = date;
    }

    public void PlusOneDay()
    {
        _currentDate.Day++;
    }
}
