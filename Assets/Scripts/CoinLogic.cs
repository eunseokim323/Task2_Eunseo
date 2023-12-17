using System;
using UnityEngine;

public class CoinLogic : MonoBehaviour
{
    [SerializeField] private char alphabet;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Touch!" + other.name);
        GiveInfo pet = FindObjectOfType<GiveInfo>();
        pet.EnqueueInfo(alphabet);
        Destroy(gameObject);
    }
}
