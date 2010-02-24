//-----------------------------------------------------------------
//  UnityTest v0.2 (2009-07-03)
//  Copyright 2009 MrJoy, Inc.
//  All rights reserved
//
//  2009-07-03 - jdf - Capture results, add basic testunner UI.
//  2009-07-02 - jdf - Initial version.
//
//-----------------------------------------------------------------
// Base class for unit tests.  Provides functionality for assertions and the 
// like.  To use it, create a sub-class, and create various public instance 
// methods with names beginning with "Test".
//
// Override "Setup" and "TearDown" to set up state that's needed across tests.
//-----------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Reflection;

public class TestResult {
	public TestResult(System.Type s, MethodInfo m, ArrayList msgs) {
		suite = s;
		method = m;
		messages = msgs;
	}

	// TODO: Capture a stack trace, but ONLY when not running on-device on iPhone.
	public System.Diagnostics.StackTrace trace;
	public System.Type suite;
	public MethodInfo method;
	public ArrayList messages;
	public int assertions, assertionsFailed;

	public bool Passed { get { return assertionsFailed == 0; } }
}

public abstract class UnityTest : MonoBehaviour {
	public static UnityTest Instance = null;

	// Enabling verbose will dump some handy debugging data to help you determine
	// what's going on wrt the UnityTest talking to UnityTestController /
	// operating independantly.  Enabling breakOnFailure will trigger the editor
	// to pause when an assertion fails, by Any Means Necessary in order to ensure
	// that you can examine the scene state in peace.
	//
	// Enabling showTestRunner will show an in-game GUI when not in the editor.
	public bool verbose = false, breakOnFailure = false, showTestRunner = true;
	public int iterations = 1;

	public virtual void Setup() {}
	public virtual void TearDown() {}

	public IEnumerator TestAll() {
		Instance = this;
		System.Type t = GetType();
		MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance);
		if((iterations > 1) && verbose)
			System.Console.WriteLine(t.Name + ": Running " + iterations + " iterations...");
		for(currentIteration = 0; currentIteration < iterations; currentIteration++) {
			// TODO: Sanity check things.  Make sure the method has no params, isn't 
			// generic, isn't abstract, returns IEnumerator, etc.

			// TODO: If return type is NOT IEnumerator, then just call the function
			// directly.

			// Order is randomized to minimize the likelihood of tests winding up 
			// being accidentally order-dependant.
			ArrayList methodsInRandomOrder = new ArrayList();
			foreach(MethodInfo m in methods)
				methodsInRandomOrder.Insert(Mathf.RoundToInt(Random.Range(0, methodsInRandomOrder.Count)), m);

			foreach(MethodInfo m in methodsInRandomOrder) {
				if(m.Name.StartsWith("Test") && (m.Name != "TestAll")) {
					ResetTestState(t, m);

					Setup();
					yield return StartCoroutine((IEnumerator)m.Invoke(this, null));
					TearDown();

					AccumulateStats();
					if(breakOnFailure && !currentResult.Passed) {
						UnityEngine.Debug.Break();
						yield break;
					}
				}
			}
		}
		Summary();
	}

	private void ResetTestState(System.Type t, MethodInfo m) {
		currentResult = new TestResult(t, m, new ArrayList());
		results.Add(currentResult);
	}

	private void AccumulateStats() {
		totalTests++;
		if(currentResult.assertionsFailed > 0) totalTestsFailed++;
		totalAssertions += currentResult.assertions;
		totalAssertionsFailed += currentResult.assertionsFailed;
	}

	public ArrayList results = new ArrayList();

	private TestResult currentResult;

	private int totalTests = 0, totalTestsFailed = 0, totalAssertions = 0, totalAssertionsFailed = 0;

	[HideInInspector]
	public int currentIteration = 0;

	public IEnumerator Start() {
		if(UnityTestController.Instance == null) {
			if(verbose)
				System.Console.WriteLine(GetType().Name + ": No UnityTestController found, proceeding solo.");
			yield return 0;
			yield return StartCoroutine(TestAll());
		} else {
			if(verbose)
				System.Console.WriteLine(GetType().Name + ": Registering with UnityTestController.");
			UnityTestController.Instance.AddSuite(this);
		}
	}

	// We choose 320x320 because it works everywhere, including iPhone.  May want
	// selectively choose a size based on what environment we're in.
	protected Rect windowRect = new Rect(0, 0, 320, 320);
	public void OnGUI() {
		if(showTestRunner && (!Application.isEditor) && (UnityTestController.Instance == null)) {
			GUILayout.BeginArea(windowRect, "Unit Tests", "window");
				DoWindow();
			GUILayout.EndArea();
		}
	}

	protected Vector2 scrollPosition = Vector2.zero;
	private static Color passColor = new Color(0f, 0.33f, 0f, 1f), failColor = new Color(0.67f, 0f, 0f, 1f);
	public void DoWindow() {
		int passes = 0, fails = 0;
		Color c = GUI.color;
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			Color oldLabelColor = GUI.skin.label.normal.textColor;
			GUI.skin.label.normal.textColor = Color.white;
			foreach(TestResult r in results) {
				string passString = "";
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

	protected void Summary() {
		int passes = 0, fails = 0;
		System.Console.Write(GetType().Name + ": ");
		foreach(TestResult r in results) {
			if(r.Passed) passes++;
			else fails++;
			System.Console.Write(r.Passed ? "." : "*");
		}
		System.Console.WriteLine("");
		foreach(TestResult r in results) {
			foreach(string msg in r.messages) {
				System.Console.WriteLine("\t" + r.method.Name + ": " + msg);
			}
		}
		if(iterations > 1)
			System.Console.WriteLine(GetType().Name + ": " + (passes+fails) + " tests in " + iterations + " iterations, " + fails + " failures.");
		else
			System.Console.WriteLine(GetType().Name + ": " + (passes+fails) + " tests, " + fails + " failures.");

		System.Console.WriteLine(GetType().Name + ": " + totalAssertions + " assertions total, " + totalAssertionsFailed + " failed.");
	}

	protected bool AssertTrue(bool condition, string msg) {
		currentResult.assertions++;
		if(!condition) {
			currentResult.assertionsFailed++;
			currentResult.messages.Add(msg);
			if(breakOnFailure) {
				// Debug.Break is insufficient because we're in a coroutine...  Doesn't
				// break too late without some assistance.  Previously I tried throwing
				// an exception here, but since we can't do try/catch around a yield
				// that isn't sufficient when we want to capture results.  So instead
				// we force the burden to the user -- Asserts must be wrapped in if
				// statements and call "yield break".
				UnityEngine.Debug.Break();
			}
		}
		return !condition;
	}

	protected bool Fail() { return Fail("Unexpected condition"); }
	protected bool Fail(string msg) { return AssertTrue(false, msg); }

	protected bool AssertFalse(bool condition) { return AssertTrue(!condition, "Expected false, but got true"); }
	protected bool AssertFalse(bool condition, string msg) { return AssertTrue(!condition, msg); }

	protected bool AssertEqual(bool expectedValue, bool actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(bool expectedValue, bool actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }

	protected bool AssertEqual(byte expectedValue, byte actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(byte expectedValue, byte actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }

	protected bool AssertEqual(short expectedValue, short actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(short expectedValue, short actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }

	protected bool AssertEqual(int expectedValue, int actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(int expectedValue, int actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }

	protected bool AssertEqual(long expectedValue, long actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(long expectedValue, long actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }

	protected bool AssertEqual(float expectedValue, float actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(float expectedValue, float actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }

	protected bool AssertEqual(double expectedValue, double actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(double expectedValue, double actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }

	protected bool AssertEqual(string expectedValue, string actualValue) { return AssertEqual(expectedValue, actualValue, "Expected " + expectedValue + ", but got " + actualValue); }
	protected bool AssertEqual(string expectedValue, string actualValue, string msg) { return AssertTrue(expectedValue == actualValue, msg); }
}
