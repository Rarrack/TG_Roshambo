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
    int waitTime = 0;
    public bool multiplayer;
    bool animate = true;
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

    GameObject playerButtons;
    GameObject playerCSprite;

    public GameObject aiSprite;
    public GameObject aiProfile;
    public GameObject aiDecision;

    GameObject enemyButtons;
    GameObject enemyCSprite;

    public GameObject duringButton;
    public GameObject endButtons;

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

    void Awake()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().PlayMusic("Battle Theme");
    }

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        gameScreen = GameObject.Find("Game Canvas");
        playerButtons = GameObject.Find("Player Buttons");
        playerCSprite = GameObject.FindGameObjectWithTag("PCSprite");
        playerCSprite.SetActive(false);
        if (multiplayer == true)
        {
            enemyButtons = GameObject.Find("Enemy Buttons");
            enemyCSprite = GameObject.FindGameObjectWithTag("ECSprite");
            enemyCSprite.SetActive(false);
        }
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
                switch (multiplayer)
                {
                    case false:
                        if (playerChoice.Selected != Choice.Choices.Undecided)
                        {
                            AndTheWinnerIs();
                            state = State.Action;
                            gameScreen.SetActive(false);
                            actionScreen.SetActive(true);
                            //DecisionDraw();
                            //AttackDefend();
                        }
                        break;
                    case true:
                        if (playerChoice.Selected != Choice.Choices.Undecided && aiChoice.Selected != Choice.Choices.Undecided)
                        {
                            AndTheWinnerIs();
                            state = State.Action;
                            gameScreen.SetActive(false);
                            actionScreen.SetActive(true);
                            //DecisionDraw();
                        }   //AttackDefend();
                        break;
                }
                break;
            case State.Action:
                //Mateusz TODO:: Get animations on attack to run here depending on the attack
                //Make sure to switch state variable to State.Result at the end of the animation
                waitTime += 1;
                if(waitTime >= 30 && animate == true)
                {
                    PlayAnims(playerChoice, aiChoice, a, p);
                    DecisionDraw();
                    animate = false;
                }
                 else if (waitTime >= 120)
                {
                    waitTime = 0;
                    state = State.Result;
                    playerDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[3];
                    aiDecision.GetComponent<UnityEngine.UI.Image>().sprite = choicesSprites[3];
                    actionScreen.SetActive(false);
                    resultScreen.SetActive(true);
                    duringButton.SetActive(false);
                    endButtons.SetActive(false);
                }
                break;
            case State.Result:
                animate = true;
                if (playerPoints != 3 && aiPoints != 3)
                {
                    switch (winner)
                    {
                        case 0:
                            //Tie
                            if(multiplayer == false)
                            {
                                gameTexts[4].text = "Draw";
                            }
                            else
                            {
                                gameTexts[4].text = "Draw";
                                gameTexts[5].text = "Draw";
                            }
                            IsEnd();
                            playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
                            aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[0];
                            playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[7];
                            aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[7];
                            gameTexts[2].text = "That was so close!";
                            gameTexts[3].text = "You got lucky...";
                            winner = 3;
                            break;
                        case 1:
                            //Win
                            if (multiplayer == false)
                            {
                                gameTexts[4].text = "You Win!";
                            }
                            else
                            {
                                gameTexts[4].text = "You Win!";
                                gameTexts[5].text = "You Lose...";
                            }
                            IsEnd();
                            playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
                            aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[0];
                            playerPoints += 1;
                            gameTexts[0].text = playerPoints.ToString();
                            aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[7];
                            gameTexts[2].text = "Good game!";
                            gameTexts[3].text = "I'll win next time for sure...";
                            winner = 3;
                            break;
                        case 2:
                            //Lose
                            if (multiplayer == false)
                            {
                                gameTexts[4].text = "You Lose...";
                            }
                            else
                            {
                                gameTexts[4].text = "You Lose...";
                                gameTexts[5].text = "You Win!";
                            }
                            IsEnd();
                            playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
                            aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[0];
                            aiPoints += 1;
                            gameTexts[1].text = aiPoints.ToString();
                            playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[7];
                            gameTexts[2].text = "Ouch, that was rough...";
                            gameTexts[3].text = "Better luck next time.";
                            winner = 3;
                            break;
                        default:
                            break;
                    }
                }
                else if((playerPoints == 3 || aiPoints == 3) && endGame != true)
                {
                    GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Battle Theme");
                    if (playerPoints > aiPoints)
                    {
                        //Victory
                        int i = PlayerPrefs.GetInt("Total Wins");
                        i += 1;
                        PlayerPrefs.SetInt("Total Wins", i);
                        endGame = true;
                        IsEnd();
                        GameObject.Find("__bgm").GetComponent<BGM_Manager>().PlayMusic("Victory Theme");
                        playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[6];
                        playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[4];
                        aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[8];
                        aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[3];
                        gameTexts[2].text = "No problem for a True Hero!";
                        gameTexts[3].text = "I can't believe that I lost...";
                    }
                    else
                    {
                        //Defeat
                        endGame = true;
                        IsEnd();
                        GameObject.Find("__bgm").GetComponent<BGM_Manager>().PlayMusic("Defeat Theme");
                        playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[8];
                        playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[3];
                        aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[6];
                        aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[4];
                        gameTexts[2].text = "How could I lose to you...?";
                        gameTexts[3].text = "Did you actually think you could win?";
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
        if(multiplayer == true)
        {
            playerButtons.SetActive(false);
            playerCSprite.SetActive(true);
        }
    }

    public void Choose2(int choice)
    {
        switch (choice)
        {
            case 0:
                aiChoice.Selected = Choice.Choices.Fire;
                break;
            case 1:
                aiChoice.Selected = Choice.Choices.Earth;
                break;
            case 2:
                aiChoice.Selected = Choice.Choices.Water;
                break;
        }
        if (multiplayer == true)
        {
            enemyButtons.SetActive(false);
            enemyCSprite.SetActive(true);
        }
    }

    private void AndTheWinnerIs()
    {
        if (multiplayer != true)
        {
            aiChoice.AIChoice();
        }

        switch (playerChoice.CheckWinner(aiChoice.Selected))
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

    void PlayAnims(Choice playerChoice, Choice AiChoice, Animator a, Animator p)
    {
        switch (playerChoice.Selected)
        {
            case Choice.Choices.Fire:
                if (AiChoice.Selected == Choice.Choices.Water)
                {
                    a.Play("Anim_water");
                    GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Water Attack");
                    playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[1];
                }
                if (AiChoice.Selected == Choice.Choices.Earth)
                {
                    p.Play("Anim_fire");
                    GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Fire Attack");
                    playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[2];
                }
                break;
            case Choice.Choices.Earth:
                if (AiChoice.Selected == Choice.Choices.Water)
                {
                    p.Play("Anim_earth");
                    GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Earth Attack");
                    playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[2];
                }
                if (AiChoice.Selected == Choice.Choices.Fire)
                {
                    a.Play("Anim_fire");
                    GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Fire Attack");
                    playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[1];
                }
                break;
            case Choice.Choices.Water:
                if (AiChoice.Selected == Choice.Choices.Earth)
                {
                    a.Play("Anim_earth");
                    GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Earth Attack");
                    playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[1];
                }
                if (AiChoice.Selected == Choice.Choices.Fire)
                {
                    p.Play("Anim_water");
                    GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Water Attack");
                    playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[2];
                }
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
        playerButtons.SetActive(true);
        playerCSprite.SetActive(false);
        if(multiplayer == true)
        {
            enemyButtons.SetActive(true);
            enemyCSprite.SetActive(false);
        }
        state = State.Game;
        playerProfile.GetComponent<SpriteRenderer>().sprite = playerSprites[5];
        playerSprite.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
        aiProfile.GetComponent<SpriteRenderer>().sprite = aiSprites[5];
        aiSprite.GetComponent<SpriteRenderer>().sprite = aiSprites[0];
        gameTexts[2].text = "";
        gameTexts[3].text = "";
    }

    void ResetGame()
    {
        playerPoints = 0;
        gameTexts[0].text = "0";
        aiPoints = 0;
        gameTexts[1].text = "0";
        endGame = false;
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Victory Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Defeat Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().PlayMusic("Battle Theme");
    }

    public void MainMenu()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Victory Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Defeat Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Battle Theme");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Victory Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Defeat Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Battle Theme");
        PlayerPrefs.SetInt("Scene", 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Victory Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Defeat Theme");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Battle Theme");
        PlayerPrefs.SetInt("Scene", 2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void IsEnd()
    {
        if(endGame == true)
        {
            endButtons.SetActive(true);
            duringButton.SetActive(false);
        }
        else
        {
            endButtons.SetActive(false);
            duringButton.SetActive(true);
        }
    }
}
