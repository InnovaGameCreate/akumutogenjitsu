using System;
using UnityEngine;

public class DateManager : Singleton<DateManager>, ISaveableManager<DateSaveData>
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

    public DateSaveData EncodeToSaveData()
    {
        DateSaveData saveData = new DateSaveData();
        saveData.Month = _currentDate.Month;
        saveData.Day = _currentDate.Day;
        return saveData;
    }

    public void LoadFromSaveData(DateSaveData saveData)
    {
        _currentDate.Month = saveData.Month;
        _currentDate.Day = saveData.Day;
    }
}
