using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CharacterHealth : MonoBehaviour
{
    public int maxHealth = 3;              // Santé maximale du joueur
    private int currentHealth;             // Santé actuelle du joueur
    public float blinkDuration = 1f;       // Durée totale du clignotement
    public float blinkInterval = 0.1f;     // Intervalle du clignotement

    private SpriteRenderer spriteRenderer; // Référence au SpriteRenderer du joueur
    private bool isBlinking = false;       // Indique si le joueur est en train de clignoter

    // Référence à l'élément UI pour afficher la santé
    public TMP_Text healthText;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Mettre à jour l'UI de santé au début
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        // Ne perdre qu'un seul point de vie par collision avec un ennemi
        if (currentHealth <= 0 || amount <= 0) return;

        // Réduire la santé du joueur
        currentHealth -= amount;

        // Si la santé est à zéro ou moins, déclencher la mort du joueur
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Assurer que la santé ne soit pas négative
            UpdateHealthUI();
            Die();
        }
        else
        {
            // Mettre à jour l'UI de santé et commencer le clignotement si le joueur n'est pas déjà en train de clignoter
            UpdateHealthUI();
            if (!isBlinking)
                StartCoroutine(Blink());
        }
    }

    void Die()
    {
        // Notifier le GameManager de la mort du joueur
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }

        // Désactiver le joueur
        gameObject.SetActive(false);
    }

    IEnumerator Blink()
    {
        isBlinking = true;
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            spriteRenderer.enabled = false;             // Cache le sprite
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.enabled = true;              // Montre le sprite
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval * 2;               // Mise à jour du temps écoulé
        }

        isBlinking = false;                             // Fin du clignotement
    }

    void UpdateHealthUI()
    {
        // Si l'élément de texte pour la santé est assigné, met à jour l'affichage
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}";
        }
    }
}
