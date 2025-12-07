using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCinemachine : MonoBehaviour
{
    public GameObject Cine;
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        Cine.SetActive(true);
        Player.gameObject.SetActive(false);
    }
}
