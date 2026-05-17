namespace RigorStarter.Utilities;

public record UtilityResult(bool IsSuccess, string Message, string? ErrorDetails = null);
