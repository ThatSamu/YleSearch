using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]

public class ProgramList : MonoBehaviour {
	RectTransform contentHolder;

	float itemHeight;
	float listWidth;
	public GameObject listItem;

	public static int SinglePageItemCount = 10;
	public static int maxTitleLength = 50;
	public delegate void OnScrollBottom();
	public OnScrollBottom onScrollBottom;
	List<GameObject> allItems;

	void Awake(){
		contentHolder = transform.Find("Viewport/Content") as RectTransform;
		itemHeight = 30f;
		listWidth = contentHolder.sizeDelta.x;

		allItems = new List<GameObject>();

		transform.GetComponent<ScrollRect>().onValueChanged.AddListener(OnValueChanged);
	}

	void OnValueChanged(Vector2 newPosition){
		if(allItems.Count > 0 && newPosition.y < 0){
			onScrollBottom();
		}
	}

	public void ClearItems(){
		foreach(GameObject item in allItems){
			Destroy(item);
		}
		allItems.Clear();
	}

	public void AddItems(YleProgram[] programs){
		foreach(YleProgram program in programs){
			// Get the title of the program in finnish. If not found, skip this program.
			var programTitle = program.title.fi;
			if(programTitle == null || programTitle == ""){
				continue;
			}

			if(programTitle.Length > maxTitleLength){
				programTitle = programTitle.Substring(0, maxTitleLength) + "...";
			}

			// Instantiate a new list item, adjust the list and set object hierarchy.
			GameObject newItem = GameObject.Instantiate(listItem) as GameObject;
			allItems.Add(newItem);

			contentHolder.sizeDelta = new Vector2(listWidth, allItems.Count * itemHeight);
			newItem.transform.SetParent(contentHolder);
			// Set the correct position for the new item inside the scroll content view.
			newItem.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -itemHeight * allItems.Count);

			// Set the title of the program to the text component.
			var uiTitle = newItem.transform.Find("Title").GetComponent<Text>();
			uiTitle.text = programTitle;
		}
	}
}
