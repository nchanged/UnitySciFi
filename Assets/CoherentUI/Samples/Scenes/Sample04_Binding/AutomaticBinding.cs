#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif
using UnityEngine;
using System.Collections;
using Coherent.UI.Binding;

#if COHERENT_UNITY_STANDALONE || UNITY_EDITOR
public class AutomaticBinding : MonoBehaviour {
	
	private CoherentUIView m_View;
	private GameOptions m_GameOptions;
	
	// Use this for initialization
	void Start () {
		m_View = GetComponent<CoherentUIView>();
		
		m_GameOptions = new GameOptions {
			Backend = "Unity3D",
			Width = 1024,
			Height = 768,
			NetPort = 17777,
		};
	}
	
	[Coherent.UI.CoherentMethod("ApplyOptions", false)]
	public void ApplyOptions(GameOptions options)
	{
		m_View.View.TriggerEvent("gameConsole:Trace", options);
	}
	
	// By default, the second argument of CoherentMethod is false
	[Coherent.UI.CoherentMethod("GetLatency")]
	public int GetNetworkLatency()
	{
		// not actual latency :)
		return (int)UnityEngine.Random.Range(0, 1000);
	}
	
	[Coherent.UI.CoherentMethod("GetGameTime")]
	public int GetGameTime()
	{
		return (int)Time.time;
	}
	
	[Coherent.UI.CoherentMethod("ViewReady", true)]
	public void ViewReady()
	{
		// show the options
		m_View.View.TriggerEvent("OpenOptions", m_GameOptions);
	}
}
#endif
