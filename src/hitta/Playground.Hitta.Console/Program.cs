// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

const string callerId = "";
const string apiKey = "";
const string phoneNumberMissingOne = "070973223";
// 1. Loop and set phone number

for (int length = 2; length <= 9; length++)
{ 
    var firstPart = phoneNumberMissingOne.Substring(0, length);
    var lastPart = "";
    if (length < 9)
    {
        lastPart = phoneNumberMissingOne.Substring(length);
    }
    
    for (int number = 0; number <= 0; number++)
    {
        var currentPhoneNumber = $"{firstPart}{number}{lastPart}";
        
        // 2. Calculate auth headers
        var url = $"https://api.hitta.se/publicsearch/v1/persons?what={currentPhoneNumber}";
        using var client = new HttpClient();
        var unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var random = Guid.NewGuid().ToString("N").Substring(0, 16);
        var hash = CalculateHash(callerId, unixTime, random, apiKey);
        client.DefaultRequestHeaders.Add("hitta-callerid", callerId);
        client.DefaultRequestHeaders.Add("hitta-time", unixTime);
        client.DefaultRequestHeaders.Add("hitta-random", random);
        client.DefaultRequestHeaders.Add("hitta-hash", hash);
        
        // 3. Make API call
        Console.WriteLine("");
        Console.WriteLine($"Call for {currentPhoneNumber}");
        var response = await client.GetAsync(url);

        try
        {
            var rawBody = await response.Content.ReadAsStringAsync();
            var hittaResponse = JsonSerializer.Deserialize<HittaResponse>(rawBody);

            if (hittaResponse.Result.Persons.Total == 0)
            {
                Console.WriteLine("No match found");
                continue;
            }
            foreach (var person in hittaResponse.Result.Persons.Person)
            {
                if (person.DisplayName.Contains("Jim") || person.DisplayName.Contains("Alice"))
                {
                    Console.WriteLine("Potential match found!!!");
                }
                Console.WriteLine(person.DisplayName);
                if (person.Address != null)
                {
                    foreach (var address in person.Address)
                    {
                        Console.WriteLine(address.City);
                    }
                }
                if (person.Phone != null)
                {
                    foreach (var phoneResponse in person.Phone)
                    {
                        Console.WriteLine(phoneResponse.displayAs);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}

Console.WriteLine("Done. Exiting when press.");
Console.ReadLine();



// 4. Print each result

string CalculateHash(string callerId, string unixTime, string random, string apiKey)
{
    var input = $"{callerId}{unixTime}{apiKey}{random}"; // Payload intentionally skipped
    using var sha256Hash = SHA256.Create();
    // Convert the input string to a byte array and compute the hash.
    var data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

    // Convert the byte array to a hexadecimal string.
    var sb = new StringBuilder();
    for (int i = 0; i < data.Length; i++)
    {
        sb.Append(data[i].ToString("x2"));
    }
    var hash = sb.ToString();

    return hash;
}