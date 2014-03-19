using UnityEngine;
using System.Collections;

public interface IDentifiable {
	string ObjectId {get;set;}
	string ObjectName {get;set;}
	string UserId {get;set;}
	string MapId {get;set;}
	bool IsReady {get;set;}
	bool IsBuilding {get;set;}
}
