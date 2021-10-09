using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coins;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.AddCoins(_coins);
            Destroy(gameObject);
        }
    }
}
