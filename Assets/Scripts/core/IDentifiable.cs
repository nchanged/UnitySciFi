using UnityEngine;
using System.Collections;

public interface IDentifiable {
	string InstanceId {get;set;}
	string ObjectId {get;set;}
	string ObjectName {get;set;}
	string UserId {get;set;}
	string MapId {get;set;}
	long ReadyEstimation {get;set;}
	bool IsReady {get;set;}
	bool IsBuilding {get;set;}
}
