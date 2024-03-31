namespace Disasters.Api.Services;

public class ReliefWebDisasterResponse
{
    public int time { get; set; }
    public int took { get; set; }
    public int totalCount { get; set; }
    public int count { get; set; }
    public List<ReliefWebDisasterResponseDataItem> data { get; set; }

    public class ReliefWebDisasterResponseDataItem
    {
        public string id { get; set; }
        public int score { get; set; }
        public ReliefWebDisasterResponseFields fields { get; set; }
    }

    public class ReliefWebDisasterResponseFields
    {
        public string name { get; set; }
        public string status { get; set; }
        public string glide { get; set; }
        public List<ReliefWebDisasterResponseCountry> country { get; set; }
        public List<ReliefWebDisasterResponseType> type { get; set; }
        public string url { get; set; }
        public ReliefWebDisasterResponseDate date { get; set; }
    }

    public class ReliefWebDisasterResponseCountry
    {
        public string name { get; set; }
    }

    public class ReliefWebDisasterResponseType
    {
        public string name { get; set; }
    }

    public class ReliefWebDisasterResponseDate
    {
        public DateTime created { get; set; }
    }
}
