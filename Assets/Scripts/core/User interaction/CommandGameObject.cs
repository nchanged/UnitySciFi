using UnityEngine;
using System.Collections;

public class CommandGameObject {

	public static ICommandable GetCommandable(GameObject target)
	{
		ICommandable commandableComponent = (ICommandable)target.GetComponent(typeof(ICommandable));
		return commandableComponent; //Returns an ICommandable component, or null
	}

	public static void DispatchMoveCommand(ICommandable commandableComponent, Vector3 destination)
	{
		commandableComponent.OnMoveCommand(destination);
	}
}
