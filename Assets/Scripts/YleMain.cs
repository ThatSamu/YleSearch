using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(SearchEngine))]

public class YleMain : MonoBehaviour {

	SearchEngine yleSearch;
	public InputField searchInput;
	public ProgramList programList;

	ApiCredentials credentials;

	int offset;
	string currentSearch;

	void Awake () {

		InitLocalApiCredentials();
		
		offset = 0;
		yleSearch = transform.GetComponent<SearchEngine>();

		if(yleSearch){
			try{
				// Initialize the search engine with api credentials and proper callback.
				yleSearch.Init(credentials);
				yleSearch.onSearchReturned += HandleOnSearchReturned;
				// Set the ui related events.
				searchInput.onEndEdit.AddListener(OnNewSearchInput);
				programList.onScrollBottom += HandleOnEndOfList;
			}
			catch(UnityException e){
				Debug.Log(e);
			}
		}
		else{
			Debug.LogError("Could not find a proper search engine, failed to initialize.");
		}
	}

	void InitLocalApiCredentials(){
		TextAsset credentialsJson = Resources.Load("ApiCredentials") as TextAsset;
		credentials = JsonUtility.FromJson<ApiCredentials>(credentialsJson.text);
		Debug.Log(credentials.appID);
		Debug.Log(credentials.appKey);
	}

	void HandleOnSearchReturned (YleProgram[] programs){
		programList.AddItems(programs);
	}

	public void OnNewSearchInput(){
		OnNewSearchInput(searchInput.text);	
	}

	public void OnNewSearchInput(string searchString){
		programList.ClearItems();

		offset = 0;
		currentSearch = searchString;
		yleSearch.SearchByName(currentSearch, offset);
	}

	void HandleOnEndOfList(){
		if(!yleSearch.isRequesting){
			offset += ProgramList.SinglePageItemCount;
			yleSearch.SearchByName(currentSearch, offset);
		}
	}
}
