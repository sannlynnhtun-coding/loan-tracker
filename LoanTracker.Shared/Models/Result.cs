namespace LoanTracker.Shared.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsError { get { return !IsSuccess; } }
    public bool IsValidationError { get { return ResultType == EnumResultType.ValidationError; } }
    public bool IsNotFoundError { get { return ResultType == EnumResultType.NotFoundError; } }
    public EnumResultType ResultType { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public Result() { }

    public Result(bool isSuccess, T? data, string? message)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

    public static Result<T> Success(T data, string? message = "Operation Successful.")
    {
        return new Result<T>(true, data, message) { ResultType = EnumResultType.Success };
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(false, default, errorMessage) { ResultType = EnumResultType.Failure };
    }

    public static Result<T> ValidationError(string errorMessage)
    {
        return new Result<T>(false, default, errorMessage) { ResultType = EnumResultType.ValidationError };
    }

    public static Result<T> NotFoundError(string errorMessage = "Data not found")
    {
        return new Result<T>(false, default, errorMessage) { ResultType = EnumResultType.NotFoundError };
    }
}
