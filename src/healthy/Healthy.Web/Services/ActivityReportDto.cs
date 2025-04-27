namespace Healthy.Web.Services;

public record ActivityReportDto(
    Guid Id, 
    string Activity,
    ActivityType ActivityType,
    FeelingType FeelingType,
    DateTimeOffset Time);