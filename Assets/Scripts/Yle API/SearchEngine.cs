using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.Networking;

public struct ApiCredentials{
	public string appKey;
	public string appID;
}

public class SearchEngine : MonoBehaviour{

	public delegate void OnSearchReturned(YleProgram[] programs);
	public OnSearchReturned onSearchReturned;

	string programsAPI = "https://external.api.yle.fi/v1/programs/";

	ApiCredentials credentials;

	public bool isRequesting;

	public void Init(ApiCredentials credentials){
		if(credentials.appKey == null || credentials.appKey == ""){
			throw new UnityException("Search engine failed to init with bad app key.");
		}
		if(credentials.appID == null || credentials.appID == ""){
			throw new UnityException("Search engine failed to init with bad app id.");
		}

		this.credentials = credentials;
	}

	public void SearchByName(string name, int offset = 0){
		StartCoroutine(SearchRoutine(name, offset));
	}

	string GenerateSearchURL(string query, int offset){
		return programsAPI + "items.json?q=" + query + "&mediaobject=video&limit=10&offset=" + offset + "&app_id=" + credentials.appID + "&app_key=" + credentials.appKey;
	}

	IEnumerator SearchRoutine(string name, int offset){
		isRequesting = true;
		UnityWebRequest searchRequest = UnityWebRequest.Get(GenerateSearchURL(name, offset));

		yield return searchRequest.Send();

		isRequesting = false;

		if(searchRequest.isError || searchRequest.responseCode != 200) {
			Debug.LogError("Request failed!");
			Debug.LogError("Error: " + searchRequest.error);
			Debug.LogError("Response code: " + searchRequest.responseCode);
        }
        else {
			YleProgramsApiData programs = JsonUtility.FromJson<YleProgramsApiData>(searchRequest.downloadHandler.text);
			onSearchReturned(programs.data);
        }
	}
}
