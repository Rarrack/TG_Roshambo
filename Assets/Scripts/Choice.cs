using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice
{
    public enum Choices
    {
        Rock,
        Paper,
        Scissor,
        Undecided
    }

    Choices selected = Choices.Undecided;
    public Choices Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
        }
    }

    public void AIChoice()
    {
        int choice = Random.Range(1, 3) % 3;

        switch (choice)
        {
            case 0:
                selected = Choices.Rock;
                break;
            case 1:
                selected = Choices.Paper;
                break;
            case 2:
                selected = Choices.Scissor;
                break;
        }
    }

    public int CheckWinner(Choices AiChoice)
    {
        int outcome = 3;

        // 0 = Win, 1 = Tie, 2 = Lose
        switch (selected)
        {
            case Choices.Rock:
                if(AiChoice == Choices.Rock)
                {
                    outcome = 1;
                }
                if (AiChoice == Choices.Paper)
                {
                    outcome = 2;
                }
                if (AiChoice == Choices.Scissor)
                {
                    outcome = 0;
                }
                break;
            case Choices.Paper:
                if (AiChoice == Choices.Rock)
                {
                    outcome = 0;
                }
                if (AiChoice == Choices.Paper)
                {
                    outcome = 1;
                }
                if (AiChoice == Choices.Scissor)
                {
                    outcome = 2;
                }
                break;
            case Choices.Scissor:
                if (AiChoice == Choices.Rock)
                {
                    outcome = 2;
                }
                if (AiChoice == Choices.Paper)
                {
                    outcome = 0;
                }
                if (AiChoice == Choices.Scissor)
                {
                    outcome = 1;
                }
                break;
        }

        return outcome;
    }
}
