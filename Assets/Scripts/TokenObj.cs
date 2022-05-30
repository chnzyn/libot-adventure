using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenObj : MonoBehaviour
{
    public Backend backend;
    public int value;

    public void Start() 
    {
       backend = GameObject.Find("Blockchain").GetComponent<Backend>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            StartCoroutine(airdrop(value));
        }
        
    }

    public IEnumerator airdrop(int value)
    {
        yield return StartCoroutine(backend.AirdropTokens(value));
        Debug.Log("Airdrop finished!");
        Destroy(this.gameObject);
    }
}
