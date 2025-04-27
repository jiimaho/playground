namespace Healthy.Web.Storage;

public class ActivityReportModel
{
    public Guid Id { get; set; }
    public string Activity { get; set; } = string.Empty;
    public ActivityTypeModel ActivityType { get; set; }
    public FeelingTypeModel FeelingType { get; set; }
    public DateTimeOffset Time { get; set; }
}

public enum ActivityTypeModel
{
    Plus,
    Minus,
    Unsure
}

public enum FeelingTypeModel
{
    VeryRelaxed,
    Relaxed,
    Neutral,
    Stressed,
    VeryStressed
}