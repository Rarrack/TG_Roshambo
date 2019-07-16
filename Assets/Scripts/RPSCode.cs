using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSCode : MonoBehaviour
{
    Choice playerChoice = new Choice();
    Choice aiChoice = new Choice();
    int playerPoints = 0;
    int aiPoints = 0;
    // 0 = Tie, 1 = PlayerWin, 2 = AIWin
    int winner = 0;
    bool endGame = false;
    int testWaitTime = 0;

    public UnityEngine.UI.Text playerScore;
    public UnityEngine.UI.Text aiScore;
    public UnityEngine.UI.Text playerResult;
    public UnityEngine.UI.Text aiResult;

    GameObject gameScreen;
    GameObject actionScreen;
    GameObject resultScreen;

    enum State
    {
        Game,
        Action,
        Result
    }

    State state = State.Game;

    // Start is called before the first frame update
    void Start()
    {
        gameScreen = GameObject.Find("Game Canvas");
        actionScreen = GameObject.Find("Action Canvas");
        actionScreen.SetActive(false);
        resultScreen = GameObject.Find("Result Screen");
        resultScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Game:
                if (playerChoice.Selected != Choice.Choices.Undecided)
                {
                    AndTheWinnerIs();
                    state = State.Action;
                    gameScreen.SetActive(false);
                    actionScreen.SetActive(true);
                    Debug.Log("Action Screen Initiated");
                }
                break;
            case State.Action:
                testWaitTime += 1;
                if (testWaitTime >= 100)
                {
                    testWaitTime = 0;
                    state = State.Result;
                    actionScreen.SetActive(false);
                    resultScreen.SetActive(true);
                    Debug.Log("Result Screen Initiated");
                }
                break;
            case State.Result:
                if(playerPoints != 3 && aiPoints != 3)
                {
                    switch (winner)
                    {
                        case 0:
                            //Tie
                            playerResult.text = "A Tie?!";
                            aiResult.text = "A Tie?!";
                            break;
                        case 1:
                            //Win
                            playerResult.text = "Point for me!";
                            aiResult.text = "You win this round...";
                            playerPoints += 1;
                            playerScore.text = playerPoints.ToString();
                            break;
                        case 2:
                            //Lose
                            playerResult.text = "Darnit";
                            aiResult.text = "Oh yeah!";
                            aiPoints += 1;
                            aiScore.text = aiPoints.ToString();
                            break;
                    }
                }
                else
                {
                    if (playerPoints > aiPoints)
                    {
                        //Victory
                        playerResult.text = "Good game!";
                        aiResult.text = "I'll win next time...";
                        endGame = true;
                    }
                    else
                    {
                        //Defeat
                        playerResult.text = "Ouch, that was rough...";
                        aiResult.text = "Better luck next time!";
                        endGame = true;
                    }
                }
                break;
        }
        #region Old Code
        /*
        if (playerPoints != 3 || aiPoints != 3)
        {
            if (playing == true)
            {
                if (playerChoice.Selected != Choice.Choices.Undecided)
                {
                    AndTheWinnerIs();
                }
            }
            else
            {
                switch (winner)
                {
                    case 0:
                        //result.text = "Tie";
                        playerResult.text = "A Tie?!";
                        aiResult.text = "A Tie?!";
                        playing = true;
                        break;
                    case 1:
                        playerResult.text = "Point for me!";
                        aiResult.text = "You win this round...";
                        playing = true;
                        break;
                    case 2:
                        playerResult.text = "Darnit";
                        aiResult.text = "Oh yeah!";
                        playing = true;
                        break;
                }
            }
        }
        else
        {
            if(playerPoints > aiPoints)
            {
                playerResult.text = "Good game!";
                aiResult.text = "I'll win next time...";
            }
            else
            {
                playerResult.text = "Ouch, that was rough...";
                aiResult.text = "Better luck next time!";
            }
        }
        */
        #endregion
    }

    public void Choose(int choice)
    {
        switch(choice)
        {
            case 0:
                playerChoice.Selected = Choice.Choices.Rock;
                break;
            case 1:
                playerChoice.Selected = Choice.Choices.Paper;
                break;
            case 2:
                playerChoice.Selected = Choice.Choices.Scissor;
                break;
        }

        Debug.Log("You Chose: " + playerChoice.Selected.ToString());
    }

    private void AndTheWinnerIs()
    {
        aiChoice.AIChoice();
        Debug.Log("Enemy Picked: " + aiChoice.Selected.ToString());

        switch (playerChoice.CheckWinner(aiChoice.Selected))
        {
            case 0:
                Debug.Log("You Win!!!");
                winner = 1;
                break;
            case 1:
                Debug.Log("Tie");
                winner = 0;
                break;
            case 2:
                Debug.Log("You Lose");
                winner = 2;
                break;
            default:
                Debug.Log("Uh Oh...");
                break;
        }

        playerChoice.Selected = Choice.Choices.Undecided;
        aiChoice.Selected = Choice.Choices.Undecided;
    }

    public void BackToGame(bool back)
    {
        if(endGame == true)
        {
            ResetGame();
        }
        resultScreen.SetActive(back);
        gameScreen.SetActive(true);
        state = State.Game;
        Debug.Log("Game Screen Initiated");
    }

    public void ResetGame()
    {
        playerPoints = 0;
        playerScore.text = "0";
        aiPoints = 0;
        aiScore.text = "0";
    }
}
