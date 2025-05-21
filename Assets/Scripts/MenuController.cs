#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Unity.Services.Core;

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
