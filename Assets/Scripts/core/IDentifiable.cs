using UnityEngine;
using System.Collections;

public interface IDentifiable {
	string GameObjectId {get;set;}
	string UserId {get;set;}
	string MapId {get;set;}
}
