using DisastersTemp;
using Xunit;

namespace Disasters.IntegrationTests.Steps;

[Binding]
public sealed class CalculatorStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    
    private readonly Calculator _calculator = new();
    private int _result;

    public CalculatorStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("the first number is (.*)")]
    public void GivenTheFirstNumberIs(int number)
    {
        _calculator.Include(number);
    }

    [Given("the second number is (.*)")]
    public void GivenTheSecondNumberIs(int number)
    {
        _calculator.Include(number);
    }
    
    [Given(@"adding (.*) as well")]
    public void GivenAddingAsWell(int number)
    {
        _calculator.Include(number);
    }

    
    [When(@"all numbers are summarized")]
    public void WhenAllNumbersAreSummarized()
    {
        _result = _calculator.Add();
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int result)
    {
        Assert.Equal(result, _result);
    }
}