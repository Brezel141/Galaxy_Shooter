using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Public or private variable
    // Public variable can be accessed from the Unity Editor
    // Private variable can only be accessed from the script
    // Data type: int, float, double, string, bool
    // Every variable has a name

    // Speed variable
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;

    // Powerup variables
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _speedPowerupPrefab;
    // Powerup true or false variables
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    // Fire rate variable
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    // Lives variable
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
 
    // Visual effects variables
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    // Score variable
    [SerializeField]
    private int _score;
    
    // UI Manager variable
    private UIManager _uiManager;

    // Audio variables
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Take the current position of the player = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        // connect the player script to the Spawn Manager an
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
        {
            FireLaser();
        }
#elif UNITY_IOS
       if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
        {
            FireLaser();
        }
#else
         if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
#endif
    }

    void CalculateMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");  //Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");      //Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // move the player
        transform.Translate(direction * _speed * Time.deltaTime);


        float topBorder = 0.0f;
        float bottomBorder = -3.8f;

        if (transform.position.y >= topBorder)
        {
            transform.position = new Vector3(transform.position.x, topBorder, 0);
        }
        else if (transform.position.y <= bottomBorder)
        {
            transform.position = new Vector3(transform.position.x, bottomBorder, 0);
        }

        float rightBorder = 11.3f;
        float leftBorder = -11.3f;

        if (transform.position.x >= rightBorder)
        {
            transform.position = new Vector3(leftBorder, transform.position.y, 0);
        }
        else if (transform.position.x <= leftBorder)
        {
            transform.position = new Vector3(rightBorder, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        // play the laser sound effect
        _audioSource.Play();
        

    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            //turn off the shield visualizer
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TrpleShotPowerDownRoutine());
    }

    IEnumerator TrpleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        //turn on the shield visualizer
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}

