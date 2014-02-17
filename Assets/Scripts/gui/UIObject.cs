using UnityEngine;
using System.Collections;

public class UIObject {
	private int width;
	private int height;
	private string name;
	private string resourceName;
	private Vector2 position;

	public bool needtoCreate = false;
	public bool needtoUpdate = false;
	public bool needtoRemove = false;

	public UIObject(string resourceName, string name)
	{
		this.resourceName = resourceName;
		this.name = name;
	}

	public UIObject(string resourceName, string name, int width, int height)
	{
		this.resourceName = resourceName;
		this.name = name;
		this.width = width;
		this.height = height;
	}

	public void setPosition(Vector2 position)
	{
		this.position = position;
	}

	public Vector2 getPosition()
	{
		return this.position;
	}

	public void setWidth(int w){
		this.width = w;
	}

	public int getWidth(){
		return this.width;
	}

	public void setHeight(int h)
	{
		this.height = h;
	}

	public int getHeight(){
		return this.height;
	}

	public string getName()
	{
		return name;
	}
	public void setName(string name)
	{
		this.name = name;
	}
	public string getResourceName()
	{
		return resourceName;
	}
	public void setResourceName(string resourceName)
	{
		this.resourceName = resourceName;
	}

}
