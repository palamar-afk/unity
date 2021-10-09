using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Door : MonoBehaviour
{
    [SerializeField] private int _coinsToLevel2;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null && player.CoinsAmount >= _coinsToLevel2)
        {
            SceneManager.LoadScene("Level_2");
        }
    }
}
