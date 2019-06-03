using System.IO;
using UnityEngine;

namespace GracesGames.SimpleFileBrowser.Scripts
{
    public class DemoCaller : MonoBehaviour
    {
		public GameObject FileBrowserPrefab;

		public string[] FileExtensions;

		public void OpenFileBrowser() => OpenFileBrowser(FileBrowserMode.Load);

		private void OpenFileBrowser(FileBrowserMode fileBrowserMode)
        {
			GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
			fileBrowserObject.name = "FileBrowser";
			FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
			fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);
			fileBrowserScript.OpenFilePanel(FileExtensions);
			fileBrowserScript.OnFileSelect += LoadFileUsingPath;
		}

		private void LoadFileUsingPath(string path) {
			if (path.Length != 0) {
				FileStream file = File.OpenRead(path);
				file.Close();
			} else {
				Debug.Log("Invalid path given");
			}
		}
	}
}