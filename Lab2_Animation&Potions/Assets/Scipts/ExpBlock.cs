using UnityEngine;
public class ExpBlock : MonoBehaviour
{
    [SerializeField] private int _expPoints;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.AddExp(_expPoints);
            Destroy(gameObject);
        }
    }
}
