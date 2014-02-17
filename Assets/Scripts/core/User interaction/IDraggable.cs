using UnityEngine;
using System.Collections;

public interface IDraggable {
	bool OnDragMove (Vector3 position);
	void OnDragStop();
	bool CurrentPositionValid { get; set; }
}
