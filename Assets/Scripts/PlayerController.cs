using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public float speed = 10f;
    public float customAngularDamping = 0.95f; // 1 = pas d'amortissement, 0 = arrêt immédiat
    public TextMeshProUGUI countText;
    private int count = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCountText();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        // Mouvement
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        // Appliquer un "angular damping" personnalisé
        rb.angularVelocity *= customAngularDamping;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            GameController.Instance.capturePickUp(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Player.setScore(count);
            Destroy(gameObject);
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}
