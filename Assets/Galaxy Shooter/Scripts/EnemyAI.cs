using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _EnemyExplosionPrefab;
    [SerializeField]
    private AudioClip _explosionSoundClip;

    private UIManager _uiManager;
    // Use this for initialization
    void Start () {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.down *_speed * Time.deltaTime);

	    if (transform.position.y < -6.25f)
	    {
	        float randomXposition = Random.Range(-9.10f, 9.10f);
            transform.position = new Vector3(randomXposition, 8.09f, 0);
	    }
        
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //access the player
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            
            Instantiate(_EnemyExplosionPrefab, this.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_explosionSoundClip, Camera.main.transform.position, 0.1f);
            Destroy(this.gameObject);
        }
        else if (other.tag == "laserShot")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Instantiate(_EnemyExplosionPrefab, this.transform.position, Quaternion.identity);

            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_explosionSoundClip, Camera.main.transform.position, 0.1f);
            Destroy(this.gameObject);
        }
    }
    
}
