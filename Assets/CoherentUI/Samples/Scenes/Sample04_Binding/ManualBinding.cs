#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif

using UnityEngine;
using System;
using System.Collections;
using Coherent.UI.Binding;

#if COHERENT_UNITY_STANDALONE || UNITY_EDITOR
public class ManualBinding : MonoBehaviour {
	
	private CoherentUIView m_View;
	private GameOptions m_GameOptions;
	
	// Use this for initialization
	void Start () {
		m_View = GetComponent<CoherentUIView>();
		m_View.Listener.ReadyForBindings += HandleReadyForBindings;
		
		m_GameOptions = new GameOptions {
			Backend = "Unity3D",
			Width = 1024,
			Height = 768,
			NetPort = 17777,
		};
	}

	void HandleReadyForBindings (int frameId, string path, bool isMainFrame)
	{
		if (isMainFrame)
		{
			// bind ApplyOptions to "ApplyOptions" in JavaScript
			m_View.View.BindCall("ApplyOptions", (Action<GameOptions>)this.ApplyOptions);
			m_View.View.BindCall("GetLatency", (Func<int>)this.GetNetworkLatency);
			m_View.View.BindCall("GetGameTime", (Func<int>)this.GetGameTime);
			
			m_View.View.BindCall("GetMath", (Func<BoundObject>)(() => {
				return BoundObject.BindMethods(new MyMath());
			}));

			// triggered by the view when it has loaded
			m_View.View.RegisterForEvent("ViewReady", (Action)this.ViewReady);
		}
	}
	
	public void ApplyOptions(GameOptions options)
	{
		m_View.View.TriggerEvent("gameConsole:Trace", options);
	}
	
	public int GetNetworkLatency()
	{
		// not actual latency :)
		return (int)UnityEngine.Random.Range(0, 1000);
	}
	
	public int GetGameTime()
	{
		return (int)Time.time;
	}
	
	public void ViewReady()
	{
		// show the options
		m_View.View.TriggerEvent("OpenOptions", m_GameOptions);
	}
}
#endif
