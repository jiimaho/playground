// See https://aka.ms/new-console-template for more information

using MongoDB.Bson;
using MongoDB.Driver;

Console.WriteLine("Very simple example application that connects to DocumentDB in AWS (but using MongoDB driver)" +
                  "and queries a collection of a person. What I want to showcase here is the validation of the server" +
                  "certificate that is needed for a secure (TLS) connection. Working with certificates is not trivial" +
                  "hence this demo will hopfully clarify a thing or two.");
Console.WriteLine();
Console.WriteLine("There are other ways to validate a server certificate, for example you can \"install\" it in the trust" +
                  "store of the operating system. If you do this you will have to remember to also port the certificate" +
                  "if you were to deploy your application to a different machine.");


var settings = MongoClientSettings.FromConnectionString(
    "mongodb://adm:x@playground.cluster-csypmwlc2xq5.eu-central-1.docdb.amazonaws.com:27017/" +
    "?tls=true&tlsCAFile=global-bundle.pem&replicaSet=rs0&readPreference=secondaryPreferred&retryWrites=false");

settings.SslSettings = new SslSettings
{
    ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
    {
        Console.WriteLine("Callback");
        return false;
    }
};

var client = new MongoClient(settings);


var db = client.GetDatabase("");
var collection = db.GetCollection<BsonDocument>("users");

var filter = Builders<BsonDocument>.Filter.Eq("name", "Jim");

var userJim = collection.Find(filter).First();