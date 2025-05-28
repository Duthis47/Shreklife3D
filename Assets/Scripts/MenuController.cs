#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SearchService;
#endif

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }
    private static Player Joueur{ get; set; }
    public TMP_InputField InputPseudo;
    public Button playButton;
    public TMP_Text top1Text;
    public TMP_Text top2Text;
    public TMP_Text top3Text;
    public TMP_Text textPseudoTaken;


    readonly static string URLServeur = "http://shreklife3d.mygamesonline.org";


    void Start()
    {
        playButton.onClick.AddListener(OnButtonClicked);
        textPseudoTaken.alpha = 0;
        AfficherLeaderboard();
    }

    async void OnButtonClicked()
    {
        bool success = await RegisterUser(InputPseudo.text);
        if (success)
        {
            changeScene();
            // Tu peux charger une autre scène ici
            // SceneManager.LoadScene("NomDeLaScene");
        }
        else
        {
            textPseudoTaken.alpha = 1;
            Debug.LogWarning("Échec de la connexion.");
        }
    }

    public static async Task<bool> RegisterUser(string pseudo)
    {
        string URL_Connect = $"{URLServeur}/connexion.php";

        ReponseConnexion reponse = await SendPostRequest(URL_Connect, new Dictionary<string, string>()
        {
            { "pseudo", pseudo }
        });

        if (reponse != null && reponse.success)
        {
            Player.setIdPlayer(reponse.id);
            Player.setPseudo(pseudo);
            Player.setScore(0);
            return true;
        }
        else if (reponse != null && !reponse.success)
        {
            return false;
        }

        Debug.LogWarning("Connexion échouée.");
        return false;
    }
    public static async Task<bool> UpdateScoreBDD(int score, int id)
    {
        string URL_Connect = $"{URLServeur}/update.php";
        Debug.Log("" + score + "-" + id);
        bool reponse = await SendPostRequestUpdateBDD(URL_Connect);

        return reponse;
    }

    public static async Task<int> RecupClassementUser()
    {
        string URL_Connect = $"{URLServeur}/recupClassement.php";

        ReponseClassement reponse = await SendPostRequest(URL_Connect);

        if (reponse != null && reponse.success)
        {
            return reponse.place;
        }

        Debug.LogWarning("Connexion échouée.");
        return -1;
    }

    public async void AfficherLeaderboard()
    {
        List<string> leaderboard = await RecupLeaderBoard();

        if (leaderboard != null && leaderboard.Count >= 3)
        {
            top1Text.text = $"1. {leaderboard[0]}";
            top2Text.text = $"2. {leaderboard[1]}";
            top3Text.text = $"3. {leaderboard[2]}";
        }
        else
        {
            top1Text.text = "1. -";
            top2Text.text = "2. -";
            top3Text.text = "3. -";
        }
    }
    public static async Task<List<String>> RecupLeaderBoard()
    {
        string URL_Connect = $"{URLServeur}/leaderboard.php";

        ReponseLeaderboard reponse = await SendPostRequestLeaderboard(URL_Connect);

        if (reponse != null && reponse.success)
        {
            return reponse.classementsPlayers;
        }

        Debug.LogWarning("Connexion échouée.");
        return null;
    }

    public static async Task<ReponseConnexion> SendPostRequest(string url, Dictionary<string, string> data)
    {
        using (UnityWebRequest req = UnityWebRequest.Post(url, data))
        {
            await req.SendWebRequest();

            Debug.Log("Code HTTP : " + req.responseCode);
            Debug.Log("Body reçu : " + req.downloadHandler.text);

            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur : " + req.error);
                return null;
            }

            string json = req.downloadHandler.text;
            Debug.Log("Réponse JSON : " + json);

            try
            {
                return JsonUtility.FromJson<ReponseConnexion>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Erreur de parsing JSON : " + e.Message);
                return null;
            }
        }
    }

public static async Task<bool> SendPostRequestUpdateBDD(string url)
    {
        Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "id", Player.id.ToString() },
            { "score", Player.score.ToString() },
        };
        using (UnityWebRequest req = UnityWebRequest.Post(url, data))
        {
            await req.SendWebRequest();

            Debug.Log("Code HTTP : " + req.responseCode);
            Debug.Log("Body reçu : " + req.downloadHandler.text);


            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur : " + req.error);
                return false;
            }
            return true;
        }
    }
    public static async Task<ReponseClassement> SendPostRequest(string url)
    {
        Dictionary<string, string> data = new Dictionary<string, string>
        { 
            {"id", Player.id.ToString()}
        };
        using (UnityWebRequest req = UnityWebRequest.Post(url, data))
        {
            await req.SendWebRequest();

            Debug.Log("Code HTTP : " + req.responseCode);
            Debug.Log("Body reçu : " + req.downloadHandler.text);


            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur : " + req.error);
                return null;
            }

            string json = req.downloadHandler.text;
            Debug.Log("Réponse JSON : " + json);

            try
            {
                return JsonUtility.FromJson<ReponseClassement>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Erreur de parsing JSON : " + e.Message);
                return null;
            }
        }
    }
    public static async Task<ReponseLeaderboard> SendPostRequestLeaderboard(string url)
    {
        Dictionary<string, string> data = new Dictionary<string, string> { };
        using (UnityWebRequest req = UnityWebRequest.Post(url, data))
        {
            await req.SendWebRequest();

            Debug.Log("Code HTTP : " + req.responseCode);
            Debug.Log("Body reçu : " + req.downloadHandler.text);


            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur : " + req.error);
                return null;
            }

            string json = req.downloadHandler.text;
            Debug.Log("Réponse JSON : " + json);

            try
            {
                return JsonUtility.FromJson<ReponseLeaderboard>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Erreur de parsing JSON : " + e.Message);
                return null;
            }
        }
    }    
    public void changeScene()
    {
        SceneManager.LoadScene("MiniGame");
    }
}

[Serializable]
public class ReponseConnexion
{
    public int id;
    public bool success;
}

public class ReponseClassement
{
    public int place;
    public bool success;
}

public class ReponseLeaderboard
{
    public List<String> classementsPlayers;
    public bool success;
}