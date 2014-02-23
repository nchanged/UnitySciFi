#define COHERENT_UI_PRO_UNITY3D
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
using Coherent.UI;
using Coherent.UI.Binding;
using System.Runtime.InteropServices;
#endif

public class CoherentCameraLive : MonoBehaviour {
	#if UNITY_EDITOR || COHERENT_UNITY_STANDALONE || COHERENT_UNITY_UNSUPPORTED_PLATFORM
	private ImageData m_LiveView;
	private RenderTexture m_RenderTexture;
	private Texture2D m_Texture;

	public void SetTextures(ImageData imgData, RenderTexture rt, Texture2D tex)
	{
		m_LiveView = imgData;
		m_RenderTexture = rt;
		m_Texture = tex;
	}

	void OnPostRender() {
		if (m_LiveView != null)
		{
			RenderTexture currentRT = RenderTexture.active;
			RenderTexture.active = m_RenderTexture;
			m_Texture.ReadPixels(new Rect(0, 0, m_Texture.width, m_Texture.height), 0, 0);
			RenderTexture.active = currentRT;

			var data = m_Texture.GetPixels32(0);
			var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			m_LiveView.Update(dataHandle.AddrOfPinnedObject(), true);
			dataHandle.Free ();
		}
	}

	#endif
}
