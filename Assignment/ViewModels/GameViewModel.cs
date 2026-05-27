using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReCAI.Models;

namespace ReCAI.ViewModels;

public class GameViewModel : INotifyPropertyChanged
{
    private int currentIndex;
    private int score;
    private GameItem? currentItem;
    private string feedbackMessage = string.Empty;
    private bool isGameFinished;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<GameItem> Items { get; } = new();

    public GameItem? CurrentItem
    {
        get => currentItem;
        set
        {
            currentItem = value;
            OnPropertyChanged();
        }
    }

    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnPropertyChanged();
        }
    }

    public string FeedbackMessage
    {
        get => feedbackMessage;
        set
        {
            feedbackMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsGameFinished
    {
        get => isGameFinished;
        set
        {
            isGameFinished = value;
            OnPropertyChanged();
        }
    }

    public GameViewModel()
    {
        LoadItems();
        StartGame();
    }

    private void LoadItems()
    {
        Items.Clear();

        Items.Add(new GameItem
        {
            Name = "Banana Peel",
            ImageName = "item_banana_peel.png",
            CorrectBin = "Brown",
            Explanation = "Banana peels belong in the brown organic bin."
        });

        Items.Add(new GameItem
        {
            Name = "Apple Core",
            ImageName = "item_apple_core.png",
            CorrectBin = "Brown",
            Explanation = "Apple cores belong in the brown organic bin."
        });

        Items.Add(new GameItem
        {
            Name = "Can",
            ImageName = "item_can.png",
            CorrectBin = "Orange",
            Explanation = "Metal cans belong in the orange recycling bin."
        });

        Items.Add(new GameItem
        {
            Name = "Juice Carton",
            ImageName = "item_juice_carton.png",
            CorrectBin = "Orange",
            Explanation = "Drink cartons belong in the orange recycling bin."
        });

        Items.Add(new GameItem
        {
            Name = "Toothpaste Tube",
            ImageName = "item_toothpaste_tube.png",
            CorrectBin = "Orange",
            Explanation = "Empty toothpaste tubes belong in the orange recycling bin."
        });

        Items.Add(new GameItem
        {
            Name = "Yogurt Cup",
            ImageName = "item_yogurt_cup.png",
            CorrectBin = "Orange",
            Explanation = "Empty yogurt cups belong in the orange recycling bin."
        });

        Items.Add(new GameItem
        {
            Name = "Cereal Box",
            ImageName = "item_cereal_box.png",
            CorrectBin = "Blue",
            Explanation = "Cardboard boxes belong in the blue recycling bin."
        });

        Items.Add(new GameItem
        {
            Name = "Newspaper",
            ImageName = "item_newspaper.png",
            CorrectBin = "Blue",
            Explanation = "Newspapers belong in the blue recycling bin."
        });

        Items.Add(new GameItem
        {
            Name = "Plastic Bottle",
            ImageName = "item_plastic_bottle.png",
            CorrectBin = "Orange",
            Explanation = "Plastic bottles belong in the orange recycling bin."
        });

        Items.Add(new GameItem
        {
            Name = "Glass Bottle",
            ImageName = "item_glass_bottle.png",
            CorrectBin = "Purple",
            Explanation = "Glass bottles belong in the purple recycling bin."
        });
    }

    public void StartGame()
    {
        currentIndex = 0;
        Score = 0;
        IsGameFinished = false;
        FeedbackMessage = string.Empty;

        if (Items.Count > 0)
        {
            CurrentItem = Items[currentIndex];
        }
        else
        {
            CurrentItem = null;
            IsGameFinished = true;
            FeedbackMessage = "No game items were found.";
        }
    }

    public bool CheckAnswer(string selectedBin)
    {
        if (CurrentItem == null || IsGameFinished)
            return false;

        bool isCorrect = selectedBin == CurrentItem.CorrectBin;

        if (isCorrect)
        {
            Score++;
            FeedbackMessage = "Correct!";
        }
        else
        {
            FeedbackMessage = CurrentItem.Explanation;
        }

        return isCorrect;
    }

    public void MoveToNextItem()
    {
        currentIndex++;

        if (currentIndex >= Items.Count)
        {
            IsGameFinished = true;
            CurrentItem = null;
            FeedbackMessage = $"Game finished! Your score is {Score}/{Items.Count}.";
            return;
        }

        CurrentItem = Items[currentIndex];
        FeedbackMessage = string.Empty;
    }

    public void RestartGame()
    {
        StartGame();
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}