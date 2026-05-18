using RigorStarter.Shared.Models;

namespace RigorStarter.Shared.Models;

public record UtilityResult(bool IsSuccess, string Message, string? ErrorDetails = null);
