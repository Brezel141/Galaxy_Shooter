using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    // handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _liveImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private float _flickerSpeed = 0.5f;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private bool _isGameOver = false;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {

        //assign the handle to the component
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        //display image sprite
        //give it a new one based on the current lives index
        _liveImage.sprite = _livesSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(TextFlickerRoutine());
    }

    IEnumerator TextFlickerRoutine()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        while (true)
        {
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
            _restartText.gameObject.SetActive(!_restartText.gameObject.activeSelf);
            yield return new WaitForSeconds(_flickerSpeed);
        }
    }
}