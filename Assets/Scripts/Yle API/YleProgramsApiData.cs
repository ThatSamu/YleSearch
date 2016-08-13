[System.Serializable]
public struct YleProgramsApiData {
	public YleProgram[] data;
}

[System.Serializable]
public struct YleProgram{
	public YleTitle title;
}

[System.Serializable]
public struct YleTitle{
	public string fi;
	public string sv;
}

