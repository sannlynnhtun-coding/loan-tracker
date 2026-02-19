namespace LoanTracker.Shared.Models;

public class DashboardResponse
{
    public decimal TotalPortfolio { get; set; }
    public int ActiveLoansCount { get; set; }
    public int LatePaymentsCount { get; set; }
    public int NewRequestsCount { get; set; }
    public List<RecentActivityResponse> RecentActivities { get; set; } = new();
}

public class RecentActivityResponse
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string TimeAgo { get; set; } = null!;
    public ActivityType Type { get; set; }
}

public enum ActivityType
{
    PaymentReceived,
    LoanCreated,
    CustomerAdded,
    LateFeeCharged
}
