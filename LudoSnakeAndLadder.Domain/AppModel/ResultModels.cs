using LudoSnakeAndLadder.Domain.ResponseMOdel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoSnakeAndLadder.Domain.AppModel;
public class Result<T>
{
    public bool IsSuccess { get; set; }

    public bool IsError { get { return !IsSuccess; } }

    public bool IsValidationError { get { return Type == EnumRespType.ValidationError; } }

    public bool IsSystemError { get { return Type == EnumRespType.SystemError; } }

    public bool IsDataError { get { return Type == EnumRespType.Error; } }

    public bool IsNotFound { get { return Type == EnumRespType.NotFound; } }


    private EnumRespType Type { get; set; }

    public T Data { get; set; }

    public string Message { get; set; }

    public static Result<T> Success(T data, string message = "Success")
    {
        return new Result<T>()
        {
            IsSuccess = true,
            Type = EnumRespType.Success,
            Data = data,
            Message = message
        };
    }
    public static Result<T> GameOver(string message = "The Game Is Over", T? data = default)
    {
        return new Result<T>()
        {
            IsSuccess = true,
            Type = EnumRespType.Success,
            Data = data,
            Message = message
        };
    }
    public static Result<T> ValidationError(string message = "Data Validation Error Occur", T? data = default)
    {
        return new Result<T>()
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            Type = EnumRespType.ValidationError,
        };
    }
    public static Result<T> SystemError(string message = "System Error Occur", T? data = default)
    {
        return new Result<T>()
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            Type = EnumRespType.SystemError,
        };
    }
    public static Result<T> Error(string message = "Some Error Occured", T? data = default)
    {
        return new Result<T>()
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            Type = EnumRespType.Error
        };
    }
    public static Result<T> NotFoundError(string message = "NotFound Error Occured", T? data = default)
    {
        return new Result<T>()
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            Type = EnumRespType.NotFound
        };
    }
    public static Result<T> PlayerNotFoundError(string message = "Player Not Found Error Occur", T? data = default)
    {
        return new Result<T>()
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            Type = EnumRespType.PlayerNotFound
        };
    }

    public static Result<T> GameSetUpError(string message = "Game set up Error Occur", T? data = default)
    {
        return new Result<T>()
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            Type = EnumRespType.GameSetUpError
        };
    }
}
public enum EnumRespType
{
    None,
    Success,
    Error,
    ValidationError,
    SystemError,
    NotFound,
    PlayerNotFound,
    GameSetUpError,
    GameOver
}
