#if UNITY_NACL || UNITY_WEBPLAYER
#define COHERENT_UNITY_UNSUPPORTED_PLATFORM
#endif

using Coherent.UI.Binding;

// all properties / fields for Options will be visible to Coherent UI
[CoherentType(PropertyBindingFlags.All)]
public struct GameOptions
{
	public string Backend;
	public uint Width;
	public uint Height;

	public string Username
	{
		get {
		#if COHERENT_UNITY_UNSUPPORTED_PLATFORM
			return "<Unsupported platform>";
		#else
			return System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
		#endif
		}
	}

	// rename the NetPort property to NetworkPort
	[CoherentProperty("NetworkPort")]
	public uint NetPort { get; set; }
}
