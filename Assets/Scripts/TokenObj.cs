using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenObj : MonoBehaviour
{
    public Backend backend;

    public void Start() 
    {
       backend = GameObject.Find("Blockchain").GetComponent<Backend>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            backend.AirdropTokens(1);
            Destroy(this.gameObject);
        }
        
    }
}
