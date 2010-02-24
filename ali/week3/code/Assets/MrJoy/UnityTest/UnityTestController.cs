//-----------------------------------------------------------------
//  UnityTestController v0.2 (2009-07-03)
//  Copyright 2009 MrJoy, Inc.
//  All rights reserved
//
//  2009-07-03 - jdf - Capture results and add testrunner UI.
//  2009-07-02 - jdf - Initial version.
//
//-----------------------------------------------------------------
// A utility to iterate through all the scenes in a build, executing whichever 
// test suites it encounters along the way in sequence before proceeding to the
// next scene.
//
// Drop an instance into scene 0 and enable iterateScenes to go through all your
// test scenes (as configured in your build), OR drop an instance into a scene
// with multiple UnityTest instances in it (with iterateScenes DISABLED) to
// coordinate the execution of several unit test suites.
//-----------------------------------------------------------------
using UnityEngine;
using System.Collections;

[System.Serializable]
public class SuiteResults {
	public SuiteResults(System.Type t, ArrayList r) {
		suite = t;
		results = r;
	}
	public System.Type suite;
	public ArrayList results;
}

public class UnityTestController : MonoBehaviour {
	public bool verbose = false, iterateScenes = false, showTestRunner = true;
	public static UnityTestController Instance;
	public ArrayList aggregateResults = new ArrayList();

	public void Awake() { Instance = this; }
	public void OnEnable() { Instance = this; }
	public void OnDisable() { testSuites = new ArrayList(); Instance = null; }
	public void OnApplicationQuit() { Instance = null; }
	public void OnLevelWasLoaded() { Instance = this; StartCoroutine(Start()); }

		private int selectedIndex = 0;
		private string[] labels = new string[] {};

	public void OnGUI() {
		if(showTestRunner && (!Application.isEditor)) {
			GUILayout.BeginArea(new Rect(0, 0, 320, 320), "Unit Tests", "window");
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
			GUILayout.EndArea();
		}
	}

	protected Vector2 scrollPosition = Vector2.zero;
	private static Color passColor = new Color(0f, 1f, 0f, 1f), failColor = new Color(1f, 0f, 0f, 1f);
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

	public IEnumerator Start() {
		yield return 0;
		if(verbose)
			Debug.Log("UnityTestController: Executing test suites...");

		// TODO: Randomize order to avoid order-dependancy.
		foreach(UnityTest suite in testSuites) {
			SuiteResults current = new SuiteResults(suite.GetType(), suite.results);
			aggregateResults.Add(current);
			yield return StartCoroutine(suite.TestAll());
		}

		if(iterateScenes) {
			DontDestroyOnLoad(this);
			if(Application.loadedLevel < (Application.levelCount - 1)) {
				if(verbose)
					Debug.Log("UnityTestController: Loading scene #" + (Application.loadedLevel + 1));
				Application.LoadLevel(Application.loadedLevel + 1);
			}
		}
	}

	protected ArrayList testSuites = new ArrayList();

	public void AddSuite(UnityTest suite) {
		testSuites.Add(suite);
	}
}
