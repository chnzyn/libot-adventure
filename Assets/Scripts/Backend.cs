
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using SimpleJSON;
using TMPro;

public class Backend : MonoBehaviour
{
    public string userId = "417caa21-31ab-4883-aa37-ce366fefa51a";
    public string baseUrl = "192.168.0.102:8081";
    public string api_key = "5096A0DD7A77053AE06EED605174D831F53F8C30B91CBFA963C3939BE8DAF0D7DD2609A36426562D";
    public string token_id = "51015339275010B8";
    public int tokenAmount = 0;
    public float originalFontSize = 0;
    public TMP_Text tokentext;
    public HttpClient client = new HttpClient();

    public void Start()
    {
        client = new HttpClient();
        originalFontSize = tokentext.fontSize;
        InvokeRepeating("GetTokens", 0.0f, 5.0f);
        AirdropTokens(1);
    }   

    private async Task<string> GetTokens()
    {
        Debug.Log(baseUrl + "/v1/users/" + userId + "/tokens");
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try	
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl + "/v1/users/" + userId + "/tokens");
            request.Method = HttpMethod.Get;
            request.Headers.Add("api_key", api_key);
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(baseUrl + "/v1/users/" + userId + "/tokens");
            var jsonData = JSON.Parse(responseBody);
            
            var tokens = jsonData["viewModel"];
            tokenAmount = 0;
            foreach (JSONNode token in tokens)
            {
                if(token["tokenId"] == token_id)
                {
                    tokenAmount = token["amount"];
                }
            }
            tokentext.text = tokenAmount +"";
            tokentext.fontSize = originalFontSize + tokenAmount ;
            Debug.Log(tokenAmount);
        }
        catch(HttpRequestException e)
        {
            Debug.Log("\nException Caught!");	
            Debug.Log(e.Message);
        }

        return "";
    }

    public async Task<bool> AirdropTokens(int amount)
    {
        Debug.Log("Sending tokens");
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try	
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl + "/v1/transactions/airdrop?DeveloperToken=" + token_id + "&DeveloperTokenAmount=" + amount + "&GamerId=" + userId);
            request.Method = HttpMethod.Post;
            request.Headers.Add("api_key", api_key);
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(baseUrl + "/v1/users/" + userId + "/tokens");
            var jsonData = JSON.Parse(responseBody);

            var message = jsonData["viewModel"].Value;

            Debug.Log(message);
        }
        catch(HttpRequestException e)
        {
            Debug.Log("\nException Caught!");	
            Debug.Log(e.Message);
        }

        return true;
    }
}
