namespace Disasters.GraphQL.Query;

[GraphQLDescription("A disaster that has occurred.")]
public class Disaster
{
    [GraphQLDescription("The name of the disaster.")]
    public string Name { get; init; }

    [GraphQLDescription("The location of the disaster.")]
    public string Location { get; init; }
};