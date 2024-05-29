using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _bottomScreen = -5.5f;
    private float _topScreen = 7.0f;
    private float _leftScreen = -8f;
    private float _rightScreen = 8f;

    private Player _player;
    //handle to animator componet
    private Animator _anim;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        //null check player
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
        //assign the componet
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // if bottom screen
        // respawn at top with a new random x position
        if (transform.position.y < _bottomScreen)
        {
            float randomX = Random.Range(_leftScreen, _rightScreen);
            transform.position = new Vector3(randomX, _topScreen, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            // trigger the anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
            
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            // trigger the anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }
        
    }
}
