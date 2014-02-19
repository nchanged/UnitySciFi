using UnityEngine;
using System.Collections;

public interface ICommandable {
	void OnMoveCommand (Vector3 position);
}
