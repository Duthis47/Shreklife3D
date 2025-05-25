using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public GameObject pickUp;
    public GameObject ground;
    private List<GameObject> pickUps;

    public TMP_Text pseudoPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Si aucune instance n'existe encore
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // (optionnel) persiste entre les scènes
            pickUps = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject); // Empêche d'en avoir deux
        }
    }
    private void Start()
    {
        pseudoPlayer.text = Player.pseudo;
        for (int i = 0; i < 3; i++)
        {
            addPickUp();
        }
    }
    public void addPickUp()
    {
            bool placable = true;
            Vector3 position = GetRandomPositionOnPlane(ground);
            Quaternion rotation = Quaternion.Euler(45, 45, 45);

            // Vérifier collision avec les pickUps déjà placés
            for (int j = 0; j < pickUps.Count; j++)
            {
                float distance = Vector3.Distance(pickUps[j].transform.position, position);
                if (distance < 1.0f) // seuil à adapter selon taille objet
                {
                    placable = false;
                    break;
                }
            }

            if (placable)
            {
                GameObject newObject = Instantiate(pickUp, position, rotation);
                pickUps.Add(newObject);
            }
    }
    private Vector3 GetRandomPositionOnPlane(GameObject plane, float y = 0.5f)
    {
        Bounds bounds = plane.GetComponent<Renderer>().bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, y, randomZ);
    }
    public void capturePickUp(GameObject pickUp)
    {
        pickUps.Remove(pickUp);
        addPickUp();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
