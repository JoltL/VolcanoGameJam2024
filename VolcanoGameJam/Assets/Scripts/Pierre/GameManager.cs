using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;  // Panneau du menu Game Over

    private bool isGameOver = false;

    void Start()
    {
        // Assurer que le panneau de Game Over est désactivé au début
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;

            // Afficher le panneau de Game Over
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            // Option : arrêter le temps pour "mettre en pause" le jeu
            Time.timeScale = 0f;
        }
    }

    // Redémarrer le jeu (fonction pour un bouton de redémarrage)
    public void RestartGame()
    {
        Time.timeScale = 1f;  // Réactiver le temps
        // Code pour recharger la scène (ajuster en fonction de votre configuration)
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
