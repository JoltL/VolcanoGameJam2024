// SequenceHandler.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SequenceHandler : MonoBehaviour
{
    public enum SequenceType { ZQSD, Arrows }
    public SequenceType sequenceType;

    private List<string> sequence; // Utilisation de string pour gérer les lettres et les flèches
    private int currentIndex = 0;

    public TMP_Text sequenceText;
    public float timeLimit = 5f;
    private float timeRemaining;

    // Référence au composant CharacterHealth du joueur
    private CharacterHealth playerHealth;

    private bool sequenceCompleted = false;

    [SerializeField] private Sprite[] _arrowSprite; //Ref aux sprites des 4 flèches

    void Start()
    {
        GenerateSequence();
        timeRemaining = timeLimit;

        // Trouver le joueur correspondant en fonction de sequenceType
        if (sequenceType == SequenceType.ZQSD)
        {
            // Trouver le joueur du bas (PlayerBottom)
            GameObject playerObj = GameObject.FindWithTag("PlayerBottom");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<CharacterHealth>();
            }
            else
            {
                Debug.LogError("PlayerBottom non trouvé. Assurez-vous que le joueur du bas a le tag 'PlayerBottom'.");
            }
        }
        else if (sequenceType == SequenceType.Arrows)
        {
            // Trouver le joueur du haut (PlayerTop)
            GameObject playerObj = GameObject.FindWithTag("PlayerTop");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<CharacterHealth>();
            }
            else
            {
                Debug.LogError("PlayerTop non trouvé. Assurez-vous que le joueur du haut a le tag 'PlayerTop'.");
            }
        }
    }

    void Update()
    {
        if (sequenceCompleted)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            SequenceFailed();
        }
        else
        {
            HandleInput();
        }
    }

    void GenerateSequence()
    {
        sequence = new List<string>();
        int length = Random.Range(2, 4); // Séquence de 2 à 3 touches

        if (sequenceType == SequenceType.ZQSD)
        {
            char[] keys = { 'Z', 'Q', 'S', 'D' };
            for (int i = 0; i < length; i++)
            {
                char keyChar = keys[Random.Range(0, keys.Length)];
                sequence.Add(keyChar.ToString());
            }
        }
        else if (sequenceType == SequenceType.Arrows)
        {
            KeyCode[] keys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
            for (int i = 0; i < length; i++)
            {
                KeyCode keyCode = keys[Random.Range(0, keys.Length)];
                sequence.Add(keyCode.ToString());
            }
        }

        UpdateSequenceDisplay();
    }

    void HandleInput()
    {
        if (sequenceType == SequenceType.ZQSD)
        {
            string input = Input.inputString;
            foreach (char c in input)
            {
                // Convertir en majuscule
                string keyChar = c.ToString().ToUpper();

                if (currentIndex < sequence.Count && keyChar == sequence[currentIndex])
                {
                    currentIndex++;
                    UpdateSequenceDisplay();

                    if (currentIndex >= sequence.Count)
                    {
                        SequenceSucceeded();
                    }
                }
                else
                {
                    // Mauvaise touche, réinitialiser la séquence
                    ResetSequence();
                }
            }
        }
        else if (sequenceType == SequenceType.Arrows)
        {
            if (currentIndex < sequence.Count)
            {
                string requiredKey = sequence[currentIndex];
                KeyCode keyCode = GetKeyCodeFromString(requiredKey);

                if (Input.GetKeyDown(keyCode))
                {
                    currentIndex++;
                    UpdateSequenceDisplay();

                    if (currentIndex >= sequence.Count)
                    {
                        SequenceSucceeded();
                    }
                }
                else if (Input.anyKeyDown)
                {
                    // Mauvaise touche, réinitialiser la séquence
                    ResetSequence();
                }
            }
        }
    }

    KeyCode GetKeyCodeFromString(string keyName)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName);
    }

    void UpdateSequenceDisplay()
    {
        if (sequenceText == null)
        {
            return;
        }

        sequenceText.text = "";
        for (int i = 0; i < sequence.Count; i++)
        {
            string displayChar = sequence[i];

            if (sequenceType == SequenceType.Arrows)
            {
                displayChar = GetArrowSymbol(displayChar);
            }

            if (i < currentIndex)
            {
                sequenceText.text += "<color=green>" + displayChar + "</color> ";
            }
            else
            {
                sequenceText.text += displayChar + " ";
            }
        }
    }

    string GetArrowSymbol(string keyName)
    {
        switch (keyName)
        {
            case "LeftArrow":
                return "←";
            case "RightArrow":
                return "→";
            case "UpArrow":
                return "↑";
            case "DownArrow":
                return "↓";
            default:
                return keyName;
        }
    }
    //Assigner un sprite à chaque KeyCode
    Sprite GetSpriteForKey(KeyCode key)
    {

        if (key == KeyCode.UpArrow)
        {
            return _arrowSprite[0];
        }
        else if (key == KeyCode.DownArrow)
        {
            return _arrowSprite[1];
        }
        else if (key == KeyCode.LeftArrow)
        {
            return _arrowSprite[2];
        }
        else if (key == KeyCode.RightArrow)
        {
            return _arrowSprite[3];
        }
        return null;
    }

    void SequenceSucceeded()
    {
        sequenceCompleted = true;
        DisableEnemyVisuals();
    }

    void SequenceFailed()
    {
        // Appliquer des dégâts au joueur
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }

        // Détruire l'obstacle
        Destroy(gameObject);
    }

    void ResetSequence()
    {
        currentIndex = 0;
        timeRemaining = timeLimit;
        UpdateSequenceDisplay();
    }

    void DisableEnemyVisuals()
    {
        // 1. Désactiver la collision
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // 2. Masquer le texte de séquence
        if (sequenceText != null)
        {
            sequenceText.enabled = false;
        }

        // 3. Rendre le sprite semi-transparent (50%)
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.color;
            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }

        //Debug.Log(gameObject.name + " a été désactivé visuellement après une séquence réussie.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'obstacle chevauche le joueur concerné
        if (other.gameObject == playerHealth.gameObject)
        {
            if (!sequenceCompleted)
            {
                // Appliquer des dégâts au joueur
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }

                // Détruire l'obstacle après collision
                Destroy(gameObject);
            }
        }
    }
}
