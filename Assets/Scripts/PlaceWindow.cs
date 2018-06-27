using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlaceWindow : MonoBehaviour {
	static bool hasSetWindow = false;

	int windowX = 0;
	int windowY = 0;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
	[DllImport("user32.dll", EntryPoint = "FindWindow")]
	public static extern IntPtr FindWindow(System.String className, System.String windowName);
	[DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
	public static extern IntPtr GetActiveWindow();

	public static void SetPosition(int x, int y, int resX = 0, int resY = 0) {
		SetWindowPos(GetActiveWindow(), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
		// SetWindowPos(FindWindow(null, "PhotonTutorial"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
	}
#endif

	void ParseWindowPositionFromCommandLine() {
		string[] args = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < args.Length; i++) {
			if (args [i] == "-x") {
				windowX = Int32.Parse(args [i + 1]);
			} else if (args[i] == "-y") {
				windowY = Int32.Parse(args [i + 1]);
			}
		}
	}

	// Use this for initialization
	void Awake() {
		if (hasSetWindow) return;

		ParseWindowPositionFromCommandLine();
		SetPosition(windowX, windowY);
		hasSetWindow = true;
	}
}