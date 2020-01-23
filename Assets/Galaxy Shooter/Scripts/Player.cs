using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    public bool canTripleShot = false;
    public bool hasSpeedBoost = false;
    public bool shieldActivated = false;
    public int playerId = 0;
    public int livesLeft = 3;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private GameObject _ExplosionPrefab;
    [SerializeField]
    private GameObject _ShieldGameObject;
    [SerializeField]
    private GameObject[] _Engines;

    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = 0.0f;

    [SerializeField]
    private float _speed = 5.0f;

    private UIManager _uiManager;
    private GameManager _gameManager;
    //private SpawnManager _spawnManager;
    private AudioSource laserSound;
    // Use this for initialization
    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //_spawnManager = GameObject.Find("Spawn_manager").GetComponent<SpawnManager>();
        laserSound = GetComponent<AudioSource>();

        if (!_gameManager.CoopModeEnabled) {
            transform.position = new Vector3(0, 0, 0);
        }

        if (_uiManager != null)
        {   
            _uiManager.UpdateLives(livesLeft);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerId == 1)
        {
            Movement();
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire"))
            {
                FireLaser();
            }
#else
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                FireLaser();
            }
#endif
        }

        if (playerId == 2)
        {
            {
                PlayerTwoMovement();
                if (Input.GetKeyDown(KeyCode.RightControl))
                {
                    FireLaser();
                }
            }
        }
    }
    public void Damage()
    {
        if (shieldActivated)
        {
            DeactivateShield();
            return;
        }
        livesLeft--;
        _uiManager.UpdateLives(livesLeft);
        if (livesLeft == 2) {
            _Engines[0].SetActive(true);
        }
        else if(livesLeft == 1) {
            _Engines[1].SetActive(true);
        }

        if (livesLeft < 1)
        {
            Instantiate(_ExplosionPrefab, this.transform.position, Quaternion.identity);
            
            if (_gameManager != null)
            {
                _gameManager.GameOver = true;
            }
            _uiManager.DisplayMainMenuImage(true);
            if (_gameManager.CoopModeEnabled) {
                _gameManager.ClearSceneFromExistingPlayers();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Movement()
    {
#if UNITY_ANDROID
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); 
#else
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
#endif
        if (horizontalInput > 0) {
            Debug.Log(horizontalInput);
        }
        if (hasSpeedBoost)
        {
            transform.Translate(Vector3.right * _speed * 1.5f * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * 1.5f * verticalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }
        //put boundaries on the object's movement place
        if (transform.position.y > 1)
        {
            transform.position = new Vector3(transform.position.x, 1, 0);
        }
        else if (transform.position.y < -3.95f)
        {
            transform.position = new Vector3(transform.position.x, -3.95f, 0);
        }

        if (transform.position.x > 10.95f)
        {
            transform.position = new Vector3(-10.95f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.95f)
        {
            transform.position = new Vector3(10.95f, transform.position.y, 0);
        }
    }

    private void PlayerTwoMovement()
    {
        //get the keys pressed (you can find the Axis name on Edit > ProjectSettings > Input)
        if (hasSpeedBoost)
        {

            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * 1.5f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * _speed * 1.5f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * 1.5f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * 1.5f * Time.deltaTime);
            }
        }
        else
        {
            //change object's position according to the keys pressed
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
        }
        //put boundaries on the object's movement place
        if (transform.position.y > 1)
        {
            transform.position = new Vector3(transform.position.x, 1, 0);
        }
        else if (transform.position.y < -3.95f)
        {
            transform.position = new Vector3(transform.position.x, -3.95f, 0);
        }

        if (transform.position.x > 10.95f)
        {
            transform.position = new Vector3(-10.95f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.95f)
        {
            transform.position = new Vector3(10.95f, transform.position.y, 0);
        }
    }

    private void FireLaser()
    {
        if (Time.time > _canFire)
        {
            laserSound.Play();
            if (canTripleShot)
            {
                Instantiate(_TripleShotPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.03f, 0), Quaternion.identity);
            }            
            _canFire = Time.time + _fireRate;
        }
    }

    public void TripleShotPowerUpOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void ActivateShield()
    {
        shieldActivated = true;
        _ShieldGameObject.SetActive(true);
    }
    public void DeactivateShield()
    {
        shieldActivated = false;
        _ShieldGameObject.SetActive(false);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    public void SpeedPowerUpOn()
    {
        hasSpeedBoost = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        hasSpeedBoost = false;
    }

    
}
