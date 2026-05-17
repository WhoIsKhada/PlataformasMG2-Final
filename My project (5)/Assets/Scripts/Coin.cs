using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobAmount = 0.3f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // rotación y bobbing para que se vea bien
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCoin();
            Destroy(gameObject);
        }
    }
}