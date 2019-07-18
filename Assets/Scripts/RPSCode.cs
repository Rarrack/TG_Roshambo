using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSCode : MonoBehaviour
{
    #region Decision Variables
    Choice playerChoice = new Choice();
    Choice aiChoice = new Choice();
    int playerPoints = 0;
    int aiPoints = 0;
    // 0 = Tie, 1 = PlayerWin, 2 = AIWin
    int winner = 0;
    bool endGame = false;
    int testWaitTime = 0;
    #endregion

    #region Public Game World Variables
    public GameObject[] backgrounds;

    public UnityEngine.UI.Text[] gameTexts;
    public Sprite[] playerSprites;
    public Sprite[] aiSprites;
    public Sprite[] choicesSprites;

    GameObject gameScreen;
    GameObject actionScreen;
    GameObject resultScreen;

    public GameObject playerSprite;
    public GameObject playerProfile;
    public GameObject playerDecision;

    public GameObject aiSprite;
    public GameObject aiProfile;
    public GameObject aiDecision;

    public Animator a;
    public Animator p;

    #endregion

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
        Random.InitState(System.DateTime.Now.Millisecond);
        gameScreen = GameObject.Find("Game Canvas");
        actionScreen = GameObject.Find("Action Canvas");
        actionScreen.SetActive(false);
        resultScreen = GameObject.Find("Result Screen");
        resultScreen.SetActive(false);

        SetBackground();
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
                    DecisionDraw();
                }
                break;
            case State.Action:
                //Mateusz TODO:: Get animations on attack to run here depending on the attack
                //Make sure to switch state variable to State.Result at the end of the animation
                testWaitTime += 1;
                if (testWaitTime >= 100)
                {
                    testWaitTime = 0;
                    state = State.Result;
                    actionScreen.SetActive(false);
                    resultScreen.SetActive(true);
                    
                }
                break;
            case State.Result:
                if(playerPoints != 3 && aiPoints != 3)
                {
                    switch (winner)
                    {
                        case 0:
                            //Tie
                            playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[7];
                            aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[7];
                            gameTexts[4].text = "A Tie?!";
                            gameTexts[5].text = "A Tie?!";
                            winner = 3;
                            break;
                        case 1:
                            //Win
                            playerPoints += 1;
                            gameTexts[0].text = playerPoints.ToString();
                            gameTexts[1].text = playerPoints.ToString();
                            aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[7];
                            gameTexts[4].text = "Point for me!";
                            gameTexts[5].text = "You win this round...";
                            winner = 3;
                            break;
                        case 2:
                            //Lose
                            aiPoints += 1;
                            gameTexts[2].text = aiPoints.ToString();
                            gameTexts[3].text = aiPoints.ToString();
                            playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[7];
                            gameTexts[4].text = "Darnit";
                            gameTexts[5].text = "Oh yeah!";
                            winner = 3;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (playerPoints > aiPoints)
                    {
                        //Victory
                        playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[6];
                        playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[4];
                        aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[8];
                        aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[3];
                        gameTexts[4].text = "Good game!";
                        gameTexts[5].text = "I'll win next time...";
                        endGame = true;
                    }
                    else
                    {
                        //Defeat
                        playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[8];
                        playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[3];
                        aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[6];
                        aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[4];
                        gameTexts[4].text = "Ouch, that was rough...";
                        gameTexts[5].text = "Better luck next time!";
                        endGame = true;
                    }
                }
                break;
        }
    }

    public void Choose(int choice)
    {
        switch(choice)
        {
            case 0:
                playerChoice.Selected = Choice.Choices.Fire;
                break;
            case 1:
                playerChoice.Selected = Choice.Choices.Earth;
                break;
            case 2:
                playerChoice.Selected = Choice.Choices.Water;
                break;
        }
    }

    private void AndTheWinnerIs()
    {
        aiChoice.AIChoice();

        switch (playerChoice.CheckWinner(aiChoice.Selected, a, p))
        {
            case 0:
                winner = 1;
                break;
            case 1:
                winner = 0;
                break;
            case 2:
                winner = 2;
                break;
            default:
                Debug.Log("Uh Oh...");
                break;
        }
    }

    void DecisionDraw()
    {
        switch (playerChoice.Selected)
        {
            case Choice.Choices.Fire:
                playerDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[0];
                break;
            case Choice.Choices.Earth:
                playerDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[1];
                break;
            case Choice.Choices.Water:
                playerDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[2];
                break;
        }
        switch (aiChoice.Selected)
        {
            case Choice.Choices.Fire:
                aiDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[0];
                break;
            case Choice.Choices.Earth:
                aiDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[1];
                break;
            case Choice.Choices.Water:
                aiDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[2];
                break;
        }
        playerChoice.Selected = Choice.Choices.Undecided;
        aiChoice.Selected = Choice.Choices.Undecided;
    }

    void SetBackground()
    {
        foreach(GameObject back in backgrounds)
        {
            back.SetActive(false);
        }

        if (Random.Range(1, 100) % 2 == 0)
        {
            backgrounds[Random.Range(1, 3) % 3].SetActive(true);
        }
        else
        {
            backgrounds[Random.Range(0, 2)].SetActive(true);
        }
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
        playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[5];
        playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
        aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[5];
        aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[0];
    }

    void ResetGame()
    {
        playerPoints = 0;
        gameTexts[0].text = "0";
        gameTexts[1].text = "0";
        aiPoints = 0;
        gameTexts[2].text = "0";
        gameTexts[3].text = "0";
        endGame = false;
    }
}
