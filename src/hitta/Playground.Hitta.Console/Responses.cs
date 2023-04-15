using System.Text.Json.Serialization;

public class HittaResponse
{
    [JsonPropertyName("result")]
    public Result Result { get; set; }
}

public class Result
{
    [JsonPropertyName("companies")]
    public CompaniesResponse Companies { get; set; }

    [JsonPropertyName("persons")]
    public PersonsResponse Persons { get; set; }

    [JsonPropertyName("locations")]
    public LocationsResponse Locations { get; set; }

    [JsonPropertyName("attribute")]
    public List<Attribute> Attribute { get; set; }
}

public class CompaniesResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("included")]
    public int Included { get; set; }
}

public class PersonsResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("included")]
    public int Included { get; set; }

    [JsonPropertyName("attribute")]
    public List<Attribute> Attribute { get; set; }

    [JsonPropertyName("person")]
    public List<Person> Person { get; set; }
}

public class LocationsResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("included")]
    public int Included { get; set; }
}

public class Attribute
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class Person
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; }

    [JsonPropertyName("attribute")]
    public List<Attribute> Attribute { get; set; }

    [JsonPropertyName("address")]
    public List<Address> Address { get; set; }

    [JsonPropertyName("phone")]
    public List<Phone> Phone { get; set; }
}

public class Metadata
{
    [JsonPropertyName("score")]
    public double Score { get; set; }
}

public class Address
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("usageCode")]
    public string UsageCode { get; set; }

    [JsonPropertyName("isWorkAddress")]
    public bool IsWorkAddress { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("cityPreposition")]
    public string CityPreposition { get; set; }

    [JsonPropertyName("zipcode")]
    public int Zipcode { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("entrance")]
    public string Entrance { get; set; }

    [JsonPropertyName("apartmentNo")]
    public string ApartmentNo { get; set; }

    [JsonPropertyName("community")]
    public string Community { get; set; }
}

public class Phone
{
    [JsonPropertyName("label")]
    public string label { get; set; }
    
    [JsonPropertyName("callTo")]
    public string callTo { get; set; }
    
    [JsonPropertyName("displayAs")]
    public string displayAs { get; set; }
}