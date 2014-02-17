using UnityEngine;
using System.Collections;

public interface ISelectable {
	// Use this for initialization
	void OnSelect();
	void OnDeselect();
	bool IsSelected { get; set;}
}
