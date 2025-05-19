using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player)) {
            Collect(player);
        }
    }

    private void Collect(Player player)
    {
        switch (type)
        {
            case Type.Coin:
                AudioManager.Instance.PlaySound("Coin", transform.position);
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                AudioManager.Instance.PlaySound("1Up", transform.position);
                GameManager.Instance.AddLife();
                break;

            case Type.MagicMushroom:
                AudioManager.Instance.PlaySound("Big", transform.position);
                player.Grow();
                break;

            case Type.Starpower:
                player.Starpower();
                break;
        }

        Destroy(gameObject);
    }

}
