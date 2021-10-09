using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    [SerializeField] private int _manaPoints;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.AddMana(_manaPoints);
            Destroy(gameObject);
        }
    }
}
