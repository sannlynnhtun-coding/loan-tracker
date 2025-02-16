namespace LoanTracker.Shared;

public static class DevCode
{
    public static DateTime ToDateTime(this DateTime? datetime)
    {
        return Convert.ToDateTime(datetime);
    }
}