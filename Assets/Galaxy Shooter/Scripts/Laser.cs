﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 10.0f;
    // Use this for initialization
    private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		transform.Translate(Vector3.up * _speed * Time.deltaTime);

	    if (transform.position.y > 7.2f)
	    {
	        if (transform.parent != null)
	        {
                Destroy(transform.parent.gameObject);
	        }
	        Destroy(this.gameObject);
	    }
	}
}
