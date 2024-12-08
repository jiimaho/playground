namespace Chatty.Silo.Configuration;

public static class ChattyOrleansConstants
{
    public static class Cluster
    {
        public const string ClusterId = "chatty-cluster";
        public const string ServiceId = "chatty-service";
        public const string ClusteringTableName = "ChattySilos";
        public const string Region = "eu-west-1";
    }

    public static class Storage
    {
        public const string Name = "chatty";
        public const string GrainStateTableName = "ChattyGrainState";
    }
}