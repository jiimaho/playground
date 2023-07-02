using Collections;
using Terminal.Gui;

public class MainWindow : Window
{
    public MainWindow()
    {
        Title = "Example of collections (Ctrl+Q to quit)";

        var iterationsLabel = new Label
        {
            Text = "Iterations"
        };

        var iterationsText = new TextField
        {
            X = Pos.Right(iterationsLabel) + 1,
            Width = Dim.Sized(10)
        };

        var iterationsButton = new Button
        {
            Text = "Run",
            X = Pos.Right(iterationsText) + 1
        };

        var usingIteratorLabel = new Label
        {
            Text = "Using iterator",
            Y = Pos.Bottom(iterationsLabel) + 2
        };

        var usingIteratorAfterMethodTypeLabel = new Label
        {
            Text = "...",
            X = Pos.Right(usingIteratorLabel) + 5,
            Y = usingIteratorLabel.Y
        };
        var usingIteratorAfterTakeTypeLabel = new Label
        {
            Text = "...",
            X = Pos.Right(usingIteratorAfterMethodTypeLabel) + 5,
            Y = usingIteratorLabel.Y
        };
        var usingIteratorNrExecutionsLabel = new Label
        {
            Text = "...",
            X = Pos.Right(usingIteratorAfterTakeTypeLabel) + 5,
            Y = usingIteratorLabel.Y
        };

        var usingHashsetLabel = new Label
        {
            Text = "Using HashSet",
            Y = Pos.Bottom(usingIteratorLabel) + 2
        };

        var usingHashSetAfterMethodTypeLabel = new Label
        {
            Text = "...",
            X = Pos.Right(usingHashsetLabel) + 5,
            Y = usingHashsetLabel.Y
        };
        var usingHashSetAfterTakeTypeLabel = new Label
        {
            Text = "...",
            X = Pos.Right(usingHashSetAfterMethodTypeLabel) + 5,
            Y = usingHashsetLabel.Y
        };
        var usingHashSetNrExecutionsLabel = new Label
        {
            Text = "...",
            X = Pos.Right(usingHashSetAfterTakeTypeLabel) + 5,
            Y = usingHashsetLabel.Y
        };
        
         // Handlers
        iterationsButton.Clicked += () =>
        {
            var iterations = int.Parse(iterationsText.Text.ToString());
            var result = ExplainTheYieldKeyWord.Run(iterations);
            
            usingIteratorAfterMethodTypeLabel.Text = result.First.TypeAfterMethod.ToString();
            usingIteratorAfterTakeTypeLabel.Text = result.First.TypeAfterTake.ToString();
            usingIteratorNrExecutionsLabel.Text = result.First.NumberOfExecutions.ToString();
            
            usingHashSetAfterMethodTypeLabel.Text = result.Second.TypeAfterMethod.ToString();
            usingHashSetAfterTakeTypeLabel.Text = result.Second.TypeAfterTake.ToString();
            usingHashSetNrExecutionsLabel.Text = result.Second.NumberOfExecutions.ToString();
        };
        
        
        Add(
            iterationsLabel, 
            iterationsText, 
            iterationsButton, 
            usingIteratorLabel, 
            usingIteratorAfterMethodTypeLabel, 
            usingIteratorAfterTakeTypeLabel, 
            usingIteratorNrExecutionsLabel, 
            usingHashsetLabel,
            usingHashSetAfterMethodTypeLabel,
            usingHashSetAfterTakeTypeLabel,
            usingHashSetNrExecutionsLabel);
    }
}