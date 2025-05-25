using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreenController : MonoBehaviour
{
    public static int Score;
    public TMP_Text pseudoText;
    public TMP_Text scoreText;
    public TMP_Text recordText;
    public TMP_Text ClassementText;
    public Button replayButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        replayButton.interactable = false;
        replayButton.onClick.AddListener(restartGame);
        pseudoText.text = Player.pseudo;
        scoreText.text = "Votre score est de : " + Score;
        if (Score < Player.score)
        {
            recordText.text = "Dommage, votre record est de : " + Player.score;
        }
        else if (Score == Player.score)
        {
            recordText.text = "Presque vous avez égalé votre meilleur score ! N'abandonnez pas !";
        }
        ClassementText.alpha = 0;
        HandlePlayerLose();
    }

    // Update is called once per frame
    void restartGame()
    {
        SceneManager.LoadScene("MiniGame");
    }

    private async void HandlePlayerLose()
    {
        // Met à jour le score dans la BDD
        bool success = await MenuController.UpdateScoreBDD(Player.score, Player.id);

        if (!success)
        {
            Debug.LogWarning("Échec de la mise à jour du score.");
        }
        int place = await MenuController.RecupClassementUser();
        ClassementText.text = "Votre place : " +place;
        ClassementText.alpha = 1;
        replayButton.interactable = true;
    }
}

