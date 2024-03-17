namespace Disasters.GraphQL;

[GraphQLDescription("A disaster that has occurred.")]
public class Disaster
{
    public string Name { get; init; }

    public string Location { get; init; }
};