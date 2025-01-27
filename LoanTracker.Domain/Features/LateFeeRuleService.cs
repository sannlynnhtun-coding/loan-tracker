namespace LoanTracker.Domain.Features;

public class LateFeeRuleService(AppDbContext dbContext)
{
    // Get all late fee rules
    public async Task<Result<List<LateFeeRule>>> GetAllLateFeeRuleAsync()
    {
        var rules = await dbContext.LateFeeRules.ToListAsync();
        return Result<List<LateFeeRule>>.Success(rules);
    }

    // Get late fee rule by ID
    public async Task<Result<LateFeeRule>> GetLateFeeRuleByIdAsync(int id)
    {
        var rule = await dbContext.LateFeeRules.FindAsync(id);
        if (rule == null)
            return Result<LateFeeRule>.NotFoundError();

        return Result<LateFeeRule>.Success(rule);
    }

    // Create a new late fee rule
    public async Task<Result<LateFeeRule>> CreateLateFeeRuleAsync(LateFeeRule rule)
    {
        if (rule.MinDaysOverdue < 0 || rule.LateFeeAmount < 0)
            return Result<LateFeeRule>.ValidationError("MinDaysOverdue and LateFeeAmount must be non-negative.");

        dbContext.LateFeeRules.Add(rule);
        await dbContext.SaveChangesAsync();

        return Result<LateFeeRule>.Success(rule);
    }

    // Update an existing late fee rule
    public async Task<Result<LateFeeRule>> UpdateLateFeeRuleAsync(int id, LateFeeRule updatedRule)
    {
        var rule = await dbContext.LateFeeRules.FindAsync(id);
        if (rule == null)
            return Result<LateFeeRule>.NotFoundError();

        if (updatedRule.MinDaysOverdue < 0 || updatedRule.LateFeeAmount < 0)
            return Result<LateFeeRule>.ValidationError("MinDaysOverdue and LateFeeAmount must be non-negative.");

        rule.MinDaysOverdue = updatedRule.MinDaysOverdue;
        rule.MaxDaysOverdue = updatedRule.MaxDaysOverdue;
        rule.LateFeeAmount = updatedRule.LateFeeAmount;
        await dbContext.SaveChangesAsync();

        return Result<LateFeeRule>.Success(rule);
    }

    // Delete a late fee rule
    public async Task<Result<LateFeeRule>> DeleteLateFeeRuleAsync(int id)
    {
        var rule = await dbContext.LateFeeRules.FindAsync(id);
        if (rule == null)
            return Result<LateFeeRule>.NotFoundError();

        dbContext.LateFeeRules.Remove(rule);
        await dbContext.SaveChangesAsync();

        return Result<LateFeeRule>.Success(rule, "Late fee rule deleted successfully.");
    }
}
