using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomInputs : MonoBehaviour
{
    [Header("RandomArrows")]

    private KeyCode[] _allDirections = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow }; 
    public int _sequenceNumber = 3; //Difficulté Nombre de flèche __________________________________________________________
    public List<KeyCode> _inputSequence = new List<KeyCode>();
    private int _currentIndex = 0;

    [Header("Instantiate")]

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _placement;
    [SerializeField] private int _gapInBetween = 100;

    [SerializeField] private Sprite[] _arrowSprite; //Ref aux sprites des 4 flèches
    private List<GameObject> _instanciatedArrows = new List<GameObject>();

    [SerializeField] private Animator _animator;


    private void Start()
    {
        RandomArrowsSequence();
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            KeyCode pressedKey = GetPressedKey();
            if (pressedKey != KeyCode.None)
            {
                if (pressedKey == _inputSequence[_currentIndex])
                {
                print("ok");
                    _currentIndex++;
                    Destroy(_instanciatedArrows[_currentIndex - 1]);

                    // Vérifier si la séquence est complète
                    if (_currentIndex >= _inputSequence.Count)
                    {
                        _animator.SetTrigger("Close");
                        Destroy(gameObject);
                        _currentIndex = 0;
                        _instanciatedArrows.Clear();
                        //RandomArrowsSequence();

                    }
                }
                else
                {

                 
                }
            }
        }
    }

        void RandomArrowsSequence()
        {

            _inputSequence.Clear();

            float totalWidth = (_sequenceNumber - 1) * _gapInBetween;

            for (int i = 0; i < _sequenceNumber; i++)
            {
                //Créer une séquence random
                KeyCode randomKey = _allDirections[Random.Range(0, _allDirections.Length)];
                _inputSequence.Add(randomKey);

               //Vector3 pos = new Vector3(_placement.position.x, _placement.position.y, 0);
                Quaternion rot = Quaternion.Euler(0, 90, 0);
            //Instantiate la sequence

                GameObject arrows = Instantiate(_arrowPrefab, _placement.position, rot);
                Transform Transform = arrows.GetComponent<Transform>();

                //Mettre au milieu de placement
                float zOffset = i * _gapInBetween - totalWidth / 2f;
                Transform.position = new Vector3(_placement.position.x, _placement.position.y, zOffset);

                //Ajout des prefabs pour supprimer 
                _instanciatedArrows.Add(arrows);

                //Assigner le sprite du prefab au sprite de l'arrow
                //Sprite arrowImage = arrows.GetComponent<SpriteRenderer>().sprite;
                arrows.GetComponent<SpriteRenderer>().sprite = GetSpriteForKey(randomKey);
            }

            Debug.Log("New Sequence: " + string.Join(", ", _inputSequence));
        }

    //Prendre la touche qui est cliquée
    KeyCode GetPressedKey()
    {
        foreach (KeyCode key in _allDirections)
        {
            if (Input.GetKeyDown(key))
            { return key; }
        }

        return KeyCode.None;

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
}
