using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : SingletonMB<MainController>
{
    // Start is called before the first frame update
    [Header("UI Elements")]
    public Text ProjectileNumber_Text;
    public GameObject ReplayGameButton;
    public Text FinishGame_Text;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeProjectileNumber()
    {
        ProjectileNumber_Text.text = TowerColorGameManager.Instance.ProjectileNumber.ToString();
    }

    public void BeginGameButton(int projectileNumber)
    {
        TowerColorGameManager.Instance.BeginGame(projectileNumber);

        //Set the Projectile Number Text
        ProjectileNumber_Text.text = projectileNumber.ToString();
    }

    public void GameOverController()
    {
        FinishGame_Text.text="Essaie encore \n Cliquez pour rejouer";
        ReplayGameButton.SetActive(true);
    }

    public void GameWinController()
    {
        FinishGame_Text.text = "Niveau Réussi \n Cliquez pour rejouer";
        ReplayGameButton.SetActive(true);
    }

    public void Replay()
    {
        TowerColorGameManager.Instance.ResetGame();
    }
}
