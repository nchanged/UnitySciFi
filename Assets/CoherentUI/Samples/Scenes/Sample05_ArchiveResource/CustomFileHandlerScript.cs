#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif
#if UNITY_NACL || UNITY_WEBPLAYER
#define COHERENT_UNITY_UNSUPPORTED_PLATFORM
#endif

using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using tar_cs;

#if COHERENT_UNITY_STANDALONE || UNITY_EDITOR
public class CustomFileHandlerScript : MonoBehaviour {
	
	class CustomFileHandler : Coherent.UI.FileHandler
	{
		private string m_ResourcesHost;
		private string m_ArchiveName;
		
		public CustomFileHandler(string resourcesHost, string archiveName)
		{
			m_ResourcesHost = resourcesHost.ToLower();
			m_ArchiveName = archiveName;
		}
		
		private string GetArchiveFolder()
		{
		#if UNITY_EDITOR
			// Read resources from the project folder
			var uiResources = PlayerPrefs.GetString("CoherentUIResources");
			if (uiResources == string.Empty)
			{
				Debug.LogError("Missing path for Coherent UI resources. Please select path to your resources via Edit -> Project Settings -> Coherent UI -> Select UI Folder");
				// Try to fall back to the default location
				uiResources = Path.Combine(Path.Combine(Application.dataPath, "WebPlayerTemplates"), "uiresources");
				Debug.LogWarning("Falling back to the default location of the UI Resources in the Unity Editor: " + uiResources);
                PlayerPrefs.SetString("CoherentUIResources", "WebPlayerTemplates/uiresources");
			} else {
				uiResources = Path.Combine(Application.dataPath, uiResources);
			}
			return uiResources;
#else
			// Read resources from the <executable>_Data folder
			return Path.Combine(Application.dataPath, m_ResourcesHost);
#endif
		}
		
		private bool GetFilepath(string inUrl, out string cleanUrl)
		{
			var asUri = new Uri(inUrl);
			cleanUrl = asUri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
			
			bool isFile = (asUri.Scheme == "file" || asUri.Host.ToLower() != m_ResourcesHost);
			return isFile;
		}

		public void ReadFileFromFilesystem (string cleanUrl, Coherent.UI.ResourceResponse response)
		{
#if COHERENT_UNITY_UNSUPPORTED_PLATFORM
			throw new ApplicationException("Coherent UI doesn't support the target platform!");
#else
			if (!File.Exists(cleanUrl))
			{
				response.SignalFailure();
				return;
			}

			byte[] bytes = File.ReadAllBytes(cleanUrl);

			RespondWithBytes(bytes, response);
#endif
		}

		public void RespondWithBytes (byte[] bytes, Coherent.UI.ResourceResponse response)
		{
			IntPtr buffer = response.GetBuffer((uint)bytes.Length);
			if (buffer == IntPtr.Zero)
			{
				response.SignalFailure();
				return;
			}

			Marshal.Copy(bytes, 0, buffer, bytes.Length);

			response.SignalSuccess();
		}

		public void ReadTarFile (string cleanUrl, Coherent.UI.ResourceResponse response)
		{
			string archivePath = Path.Combine(GetArchiveFolder(), m_ArchiveName);
			using (FileStream unarchFile = File.OpenRead(archivePath))
			{
				TarReader reader = new TarReader(unarchFile);
				while (reader.MoveNext(true))
				{
					var path = reader.FileInfo.FileName;
					
					if (path == cleanUrl)
					{
						using (MemoryStream ms = new MemoryStream((int)reader.FileInfo.SizeInBytes))
						{
							reader.Read(ms);
							byte[] buffer = ms.GetBuffer();
							RespondWithBytes(buffer, response);
						}
						
						break;
					}
				}
			}
		}
		
		public override void ReadFile (string url, Coherent.UI.ResourceResponse response)
		{
			string cleanUrl = string.Empty;
			bool isFile = GetFilepath(url, out cleanUrl);
			
			if (isFile)
			{
				ReadFileFromFilesystem(cleanUrl, response);
			}
			else
			{
				ReadTarFile(cleanUrl, response);
			}
		}

		public void WriteFileToFilesystem (string cleanUrl, Coherent.UI.ResourceData resource)
		{
#if COHERENT_UNITY_UNSUPPORTED_PLATFORM
			throw new ApplicationException("Coherent UI doesn't support the target platform!");
#else
			IntPtr buffer = resource.GetBuffer();
			if (buffer == IntPtr.Zero)
			{
				resource.SignalFailure();
				return;
			}

			byte[] bytes = new byte[resource.GetSize()];
			Marshal.Copy(buffer, bytes, 0, bytes.Length);
			
			try
			{
				File.WriteAllBytes(cleanUrl, bytes);
			}
			catch (IOException ex)
			{
				Console.Error.WriteLine(ex.Message);
				resource.SignalFailure();
				return;
			}

			resource.SignalSuccess();
#endif
		}

		public override void WriteFile (string url, Coherent.UI.ResourceData resource)
		{
			string cleanUrl = string.Empty;
			bool isFile = GetFilepath(url, out cleanUrl);
			if (!isFile)
			{
				Debug.LogWarning("In this sample the archive is read only!");
				resource.SignalFailure();
				return;
			}
			
			WriteFileToFilesystem(cleanUrl, resource);
		}
	}
	
	//----------------------------------------------------------------------------------
	
	void Awake()
	{
		CoherentUISystem.FileHandlerFactoryFunc = () => { return new CustomFileHandler("UIResources", "ArchiveResource.tar"); };
	}
}
#endif
