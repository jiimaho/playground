namespace Disasters.Api.Services;

public class DisasterResponse
{
    public int time { get; set; }
    public int took { get; set; }
    public int totalCount { get; set; }
    public int count { get; set; }
    public List<DisasterResponseDataItem> data { get; set; }

    public class DisasterResponseDataItem
    {
        public string id { get; set; }
        public int score { get; set; }
        public DisasterResponseFields fields { get; set; }
    }

    public class DisasterResponseFields
    {
        public string name { get; set; }
        public string status { get; set; }
        public string glide { get; set; }
        public List<DisasterResponseCountry> country { get; set; }
        public List<DisasterResponseType> type { get; set; }
        public string url { get; set; }
        public DisasterResponseDate date { get; set; }
    }

    public class DisasterResponseCountry
    {
        public string name { get; set; }
    }

    public class DisasterResponseType
    {
        public string name { get; set; }
    }

    public class DisasterResponseDate
    {
        public DateTime created { get; set; }
    }
}
