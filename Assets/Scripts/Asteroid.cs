using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the asteroid on the z axis
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            // Instantiate the explosion prefab at the asteroid's position
             Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the laser
            Destroy(other.gameObject);

            // Tell the Spawn Manager to start spawning
            _spawnManager.StartSpawning();

            // Destroy the asteroid
            Destroy(this.gameObject, 0.25f);
        }
    }
}
