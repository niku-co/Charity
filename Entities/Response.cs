namespace NikuAPI.Entities;

public readonly record struct Response(string Message, bool Success, object? Content);