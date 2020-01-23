using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{    
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _powerUpID = 0; // 0: TripleShot, 1: SpeedBoost, 2: Shield

    [SerializeField]
    private AudioClip powerUpSoundClip;
    // Update is called once per frame
    void Update () {
		transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -3.95f)
        {
            Destroy(this.gameObject);
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
                AudioSource.PlayClipAtPoint(powerUpSoundClip, Camera.main.transform.position, 0.1f);
                if (_powerUpID == 0)
                {
                    //activate triple shot
                    player.TripleShotPowerUpOn();
                }
                else if (_powerUpID == 1)
                {
                    //activate speed boost
                    player.SpeedPowerUpOn();
                }
                else
                {
                    //activate shield
                    player.ActivateShield();
                }
            }

            //destroy the power up
            Destroy(this.gameObject);
        }
    }

}
