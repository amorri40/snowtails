using UnityEditor;
using UnityEngine;
using System.Collections;

public class ResultViewer : EditorWindow {
	private static ResultViewer window = null;
	
	[MenuItem("Window/Test Results")]
	public static void ShowTestResults() {
#if UNITY_IPHONE
		if(window == null)
			window = new ResultViewer();

		window.Show(true);
#else
		window = (ResultViewer)EditorWindow.GetWindow(typeof(ResultViewer));
		window.Show();
#endif
	}

	private int selectedIndex = 0;
	private string[] labels = new string[] {};

	public void OnGUI() {
		GUILayout.Space(5);
		if((UnityTestController.Instance != null) && (UnityTestController.Instance.aggregateResults.Count > 0)) {
			UnityTestController controller = UnityTestController.Instance;
			if(labels.Length != controller.aggregateResults.Count) {
				labels = new string[controller.aggregateResults.Count];
				for(int i = 0; i < labels.Length; i++) 
					labels[i] = ((SuiteResults)(controller.aggregateResults[i])).suite.Name;
				selectedIndex = labels.Length - 1;
			}
			GUILayout.BeginHorizontal();
				GUILayout.BeginVertical(GUILayout.Width(100));
					if((labels.Length > 0) && (selectedIndex >= 0))
						selectedIndex = GUILayout.SelectionGrid(selectedIndex, labels, 1);
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
					// TODO: Carry over the iteration count...
					if((selectedIndex >= 0) && (controller.aggregateResults.Count > 0))
						DoWindow(((SuiteResults)(controller.aggregateResults[selectedIndex])).results, 1, 1);
				
				GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		} else if(UnityTest.Instance != null) {
			DoWindow(UnityTest.Instance.results, UnityTest.Instance.currentIteration, UnityTest.Instance.iterations);
		} else
			GUILayout.Label("No tests running...");
	}

	protected Vector2 scrollPosition = Vector2.zero;
	private static Color passColor = new Color(0f, 0.33f, 0f, 1f), failColor = new Color(0.67f, 0f, 0f, 1f);
	public void DoWindow(ArrayList results, int currentIteration, int iterations) {
		int passes = 0, fails = 0;
		int totalAssertions = 0, totalAssertionsFailed = 0;

		Color c = GUI.color;
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			Color oldLabelColor = GUI.skin.label.normal.textColor;
			GUI.skin.label.normal.textColor = Color.white;
			foreach(TestResult r in results) {
				string passString = "";
				totalAssertions += r.assertions;
				totalAssertionsFailed += r.assertionsFailed;

				if(r.assertions == 0) { GUI.color = passColor + failColor; passString = "..."; }
				else if(r.Passed) { GUI.color = passColor; passes++; passString = "PASS"; }
				else { GUI.color = failColor; fails++; passString = "FAIL"; }
				GUILayout.Label(r.method.Name + ": " + passString);
				foreach(string msg in r.messages)
					GUILayout.Label("\t" + msg);
			}
			GUI.skin.label.normal.textColor = oldLabelColor;
		GUILayout.EndScrollView();
		GUI.color = c;

		GUILayout.Label(totalAssertions + " assertions total, " + totalAssertionsFailed + " failed.");
		GUILayout.Label((passes+fails) + " tests, " + fails + " failures.");
		if(iterations > 1)
			GUILayout.Label("Iteration " + Mathf.Clamp(currentIteration + 1, 1, iterations) + " of " + iterations + " iterations.");
	}

	public void Update() {
		Repaint();
	}

#if UNITY_IPHONE
	public void OnEnable() { 
		if(window == null)
			window = this;
		Show(true);
	}
#endif
	public void OnCloseWindow() { 
		window = null;
		DestroyImmediate(this);
	}
}
