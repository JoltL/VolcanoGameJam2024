// ObstacleMover.cs
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 5f;               // Vitesse de déplacement initiale
    public float speedIncreaseRate = 0.1f; // Taux d'augmentation de la vitesse

    void Update()
    {
        // Déplacer l'obstacle vers la gauche (horizontalement)
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Augmenter la vitesse progressivement
        speed += speedIncreaseRate * Time.deltaTime;

        // Détruire l'obstacle s'il sort de l'écran à gauche
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}
