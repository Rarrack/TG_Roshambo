using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice
{
    public enum Choices
    {
        Fire,
        Earth,
        Water,
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
        Random.InitState(System.DateTime.Now.Millisecond);
        int choice = 0;

        if(Random.Range(1,100) % 2 == 0)
        {
            choice = Random.Range(1, 3) % 3;
        }
        else
        {
            choice = Random.Range(0, 2);
        }

        switch (choice)
        {
            case 0:
                selected = Choices.Fire;
                break;
            case 1:
                selected = Choices.Earth;
                break;
            case 2:
                selected = Choices.Water;
                break;
        }
    }

    public int CheckWinner(Choices AiChoice, Animator a, Animator p)
    {
        int outcome = 3;

        // 0 = Win, 1 = Tie, 2 = Lose
        switch (selected)
        {
            case Choices.Fire:
                if(AiChoice == Choices.Fire)
                {
                    outcome = 1;
                }
                if (AiChoice == Choices.Water)
                {
                    outcome = 2;
                    a.Play("Anim_water");
                }
                if (AiChoice == Choices.Earth)
                {
                    outcome = 0;
                    p.Play("Anim_fire");
                }
                break;
            case Choices.Earth:
                if (AiChoice == Choices.Water)
                {
                    outcome = 0;
                    p.Play("Anim_earth");
                }
                if (AiChoice == Choices.Earth)
                {
                    outcome = 1;
                }
                if (AiChoice == Choices.Fire)
                {
                    outcome = 2;
                    a.Play("Anim_fire");
                }
                break;
            case Choices.Water:
                if (AiChoice == Choices.Earth)
                {
                    outcome = 2;
                    a.Play("Anim_earth");
                }
                if (AiChoice == Choices.Fire)
                {
                    outcome = 0;
                    p.Play("Anim_water");
                }
                if (AiChoice == Choices.Water)
                {
                    outcome = 1;
                }
                break;
        }

        return outcome;
    }
}
