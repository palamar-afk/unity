using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _damagePoints;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.Damage(_damagePoints);
            Destroy(gameObject);
        }
    }
}
