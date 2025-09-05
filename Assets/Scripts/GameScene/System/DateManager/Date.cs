using System;
using UnityEngine;

[Serializable]
public struct Date
{
    /// <summary>
    /// 月ごとの日数（1月=31日、2月=28日、3月=31日...）
    /// </summary>
    private static readonly int[] DaysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    public Date(int month, int day)
    {
        Month = month;
        Day = day;
    }

    public int Month;
    public int Day;

    /// <summary>
    /// 一番最初の日
    /// </summary>
    public static readonly Date FirstDate = new(9, 6);

    /// <summary>
    /// 2つの日付の差を日数で計算する
    /// </summary>
    /// <param name="date1">日付1</param>
    /// <param name="date2">日付2</param>
    /// <returns>日数の差（絶対値）</returns>
    public static int DiffDate(Date date1, Date date2)
    {
        // 同じ日付なら0
        if (IsSameDate(date1, date2))
        {
            return 0;
        }

        // date1をより早い日付、date2をより遅い日付にする
        Date earlierDate, laterDate;
        if (IsEarlier(date1, date2))
        {
            earlierDate = date1;
            laterDate = date2;
        }
        else
        {
            earlierDate = date2;
            laterDate = date1;
        }

        return CalculateDaysBetween(earlierDate, laterDate);
    }

    /// <summary>
    /// 指定した日付に日数を加算する
    /// </summary>
    /// <param name="date">基準日付</param>
    /// <param name="days">加算する日数</param>
    /// <returns>加算後の日付</returns>
    public static Date AddDays(Date date, int days)
    {
        int newDay = date.Day + days;
        int newMonth = date.Month;

        // 日数が月をまたぐ場合の処理
        while (newDay > GetDaysInMonth(newMonth))
        {
            newDay -= GetDaysInMonth(newMonth);
            newMonth++;
            if (newMonth > 12)
            {
                newMonth = 1; // 翌年に移行
            }
        }

        // 日数がマイナスになる場合の処理
        while (newDay <= 0)
        {
            newMonth--;
            if (newMonth <= 0)
            {
                newMonth = 12; // 前年に移行
            }
            newDay += GetDaysInMonth(newMonth);
        }

        return new Date(newMonth, newDay);
    }

    /// <summary>
    /// date1がdate2より早い日付かチェック
    /// </summary>
    /// <param name="date1">日付1</param>
    /// <param name="date2">日付2</param>
    /// <returns>date1の方が早い場合true</returns>
    public static bool IsEarlier(Date date1, Date date2)
    {
        if (date1.Month != date2.Month)
        {
            return date1.Month < date2.Month;
        }
        return date1.Day < date2.Day;
    }

    /// <summary>
    /// 2つの日付が同じかチェック
    /// </summary>
    /// <param name="date1">日付1</param>
    /// <param name="date2">日付2</param>
    /// <returns>同じ日付の場合true</returns>
    public static bool IsSameDate(Date date1, Date date2)
    {
        return date1.Month == date2.Month && date1.Day == date2.Day;
    }

    /// <summary>
    /// 指定した日付の妥当性をチェック
    /// </summary>
    /// <param name="date">チェックする日付</param>
    /// <returns>有効な日付の場合true</returns>
    public static bool IsValid(Date date)
    {
        if (date.Month < 1 || date.Month > 12)
        {
            return false;
        }
        if (date.Day < 1 || date.Day > GetDaysInMonth(date.Month))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 指定した日付を文字列形式で取得する
    /// </summary>
    /// <param name="date">フォーマットする日付</param>
    /// <returns>フォーマット済み文字列（例：9/6）</returns>
    public static string Format(Date date)
    {
        return $"{date.Month}/{date.Day}";
    }

    /// <summary>
    /// 現在の日付を取得する
    /// </summary>
    /// <returns>現在の日付</returns>
    public static Date Now()
    {
        System.DateTime now = System.DateTime.Now;
        return new Date(now.Month, now.Day);
    }

    /// <summary>
    /// 2つの日付の間の日数を計算する（earlierDate < laterDate前提）
    /// </summary>
    private static int CalculateDaysBetween(Date earlierDate, Date laterDate)
    {
        int totalDays = 0;

        // 同じ月の場合
        if (earlierDate.Month == laterDate.Month)
        {
            return laterDate.Day - earlierDate.Day;
        }

        // 開始月の残り日数を加算
        totalDays += GetDaysInMonth(earlierDate.Month) - earlierDate.Day;

        // 中間の月の日数を加算
        int currentMonth = earlierDate.Month + 1;
        while (currentMonth < laterDate.Month)
        {
            if (currentMonth > 12)
            {
                currentMonth = 1; // 翌年に移行
            }
            totalDays += GetDaysInMonth(currentMonth);
            currentMonth++;
        }

        // 終了月の日数を加算
        totalDays += laterDate.Day;

        return totalDays;
    }

    /// <summary>
    /// 指定した月の日数を取得する
    /// </summary>
    /// <param name="month">月（1-12）</param>
    /// <returns>その月の日数</returns>
    public static int GetDaysInMonth(int month)
    {
        if (month < 1 || month > 12)
        {
            Debug.LogError($"無効な月が指定されました: {month}");
            return 30; // デフォルト値
        }
        return DaysInMonth[month - 1];
    }

    /// <summary>
    /// 等価演算子のオーバーロード
    /// </summary>
    public static bool operator ==(Date left, Date right)
    {
        return IsSameDate(left, right);
    }

    /// <summary>
    /// 非等価演算子のオーバーロード
    /// </summary>
    public static bool operator !=(Date left, Date right)
    {
        return !IsSameDate(left, right);
    }

    /// <summary>
    /// より小さい演算子のオーバーロード
    /// </summary>
    public static bool operator <(Date left, Date right)
    {
        return IsEarlier(left, right);
    }

    /// <summary>
    /// より大きい演算子のオーバーロード
    /// </summary>
    public static bool operator >(Date left, Date right)
    {
        return IsEarlier(right, left);
    }

    /// <summary>
    /// 以下演算子のオーバーロード
    /// </summary>
    public static bool operator <=(Date left, Date right)
    {
        return IsEarlier(left, right) || IsSameDate(left, right);
    }

    /// <summary>
    /// 以上演算子のオーバーロード
    /// </summary>
    public static bool operator >=(Date left, Date right)
    {
        return IsEarlier(right, left) || IsSameDate(left, right);
    }

    /// <summary>
    /// Equalsメソッドのオーバーライド
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is Date other)
        {
            return IsSameDate(this, other);
        }
        return false;
    }

    /// <summary>
    /// GetHashCodeのオーバーライド
    /// </summary>
    public override int GetHashCode()
    {
        return Month.GetHashCode() ^ Day.GetHashCode();
    }

    /// <summary>
    /// ToStringのオーバーライド
    /// </summary>
    public override string ToString()
    {
        return Format(this);
    }
}
