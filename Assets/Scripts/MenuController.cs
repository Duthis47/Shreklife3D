using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Collections;
using UnityEngine.Networking;
public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set;}
    public GameObject InputPseudo;
    public GameObject playButton;
    public GameObject Classement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class KeepTheScoreManager : MonoBehaviour
{
    // Remplace par ton URL API
    private string boardApiUrl = "https://keepthescore.com/api/v1/board/xxxxxxxxxxxx/players/";
    private string apiKey = "Bearer YOUR_API_KEY"; // Facultatif selon accès

    public void AddPlayer(string playerName)
    {
        StartCoroutine(AddPlayerCoroutine(playerName));
    }

    private IEnumerator AddPlayerCoroutine(string playerName)
    {
        // Création du corps JSON
        string jsonData = JsonUtility.ToJson(new PlayerData { name = playerName });

        using (UnityWebRequest request = UnityWebRequest.Post(boardApiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", apiKey); // Si requis

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur: " + request.error);
            }
            else
            {
                Debug.Log("Joueur ajouté: " + request.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    private class PlayerData
    {
        public string name;
    }
}
