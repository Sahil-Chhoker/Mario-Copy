using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    public TMPro.TMP_Text coinCountText;
    private int coinCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            FindObjectOfType<AudioManager>().Play("Pick Up Coin");
            other.gameObject.SetActive(false);

            coinCount++;

            UpdateCoinCountText();
        }
    }

    private void UpdateCoinCountText()
    {
        coinCountText.text = "" + coinCount;
    }
}
