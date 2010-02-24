//-----------------------------------------------------------------
//  TestTriggers v0.2 (2009-07-04)
//  Copyright 2009 MrJoy, Inc.
//  All rights reserved
//
//  2009-07-04 - jdf - Updated to assert correctly given that we don't use
//                     exceptions anymore.
//  2009-07-02 - jdf - Initial version.
//
//-----------------------------------------------------------------
// Example test suite to demonstrate usage of UnitTest framework by testing 
// various trigger behaviors and testing the viability of a workaround for an
// undesirable behavior.
//-----------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class TestTriggers : UnityTest {
	public TriggerTestBehavior dummyTower, dummyUnit;	

	private int callbackCount = 0;
	private bool towerEnter = false, towerExit = false, unitEnter = false, unitExit = false;

	public void Awake() {
		Instance = this;
	}

	public override void Setup() {
		callbackCount = 0;
		towerEnter = towerExit = unitEnter = unitExit = false;
	}

	public void DoEnterCallback(string name) {
		if(name == "Dummy Tower(Clone)") { towerEnter = true; callbackCount++; }
		else if(name == "Dummy Unit(Clone)") { unitEnter = true; callbackCount++; }
		else Debug.LogWarning("Unknown object name: " + name);
	}

	public void DoExitCallback(string name) {
		if(name == "Dummy Tower(Clone)") { towerExit = true; callbackCount++; }
		else if(name == "Dummy Unit(Clone)") { unitExit = true; callbackCount++; }
		else Debug.LogWarning("Unknown object name: " + name);
	}

	private bool AssertFlagStates(int callbacks, bool tEnter, bool tExit, bool uEnter, bool uExit) {
		return AssertEqual(callbacks, callbackCount) ||
		       AssertEqual(tEnter, towerEnter) ||
		       AssertEqual(tExit, towerExit) ||
		       AssertEqual(uEnter, unitEnter) ||
		       AssertEqual(uExit, unitExit);
	}
	
	public IEnumerator TestThatInstantiateInPlaceTriggersEnter() {
		TriggerTestBehavior tower = (TriggerTestBehavior)Instantiate(dummyTower, Vector3.zero, Quaternion.identity);
		TriggerTestBehavior unit = (TriggerTestBehavior)Instantiate(dummyUnit, Vector3.zero, Quaternion.identity);

		yield return new WaitForFixedUpdate();

		if(AssertFlagStates(2, true, false, true, false)) { yield break; }

		Destroy(tower.gameObject);
		Destroy(unit.gameObject);
	}

	public IEnumerator TestIncursionViaMovement() {
		Vector3 startPos = Vector3.one * 3f, endPos = Vector3.one * -3f;
		
		TriggerTestBehavior tower = (TriggerTestBehavior)Instantiate(dummyTower, Vector3.zero, Quaternion.identity);
		TriggerTestBehavior unit = (TriggerTestBehavior)Instantiate(dummyUnit, startPos, Quaternion.identity);

		float elapsedTime = 0f;
		while(elapsedTime < 0.2f) {
			elapsedTime = Mathf.Clamp01(elapsedTime + Time.fixedDeltaTime);
			unit.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime*5f);
			yield return new WaitForFixedUpdate();
		}

		if(AssertFlagStates(4, true, true, true, true)) { yield break; }

		Destroy(tower.gameObject);
		Destroy(unit.gameObject);
	}

	public IEnumerator TestExitViaDisable() {
		TriggerTestBehavior tower = (TriggerTestBehavior)Instantiate(dummyTower, Vector3.zero, Quaternion.identity);
		TriggerTestBehavior unit = (TriggerTestBehavior)Instantiate(dummyUnit, Vector3.zero, Quaternion.identity);

		yield return new WaitForFixedUpdate();

		unit.gameObject.SetActiveRecursively(false);

		yield return new WaitForFixedUpdate();

		if(AssertFlagStates(2, true, false, true, false)) { yield break; }

		Destroy(tower.gameObject);
		Destroy(unit.gameObject);
	}

	public IEnumerator TestExitWorkaround() {
		TriggerTestBehavior tower = (TriggerTestBehavior)Instantiate(dummyTower, Vector3.zero, Quaternion.identity);
		TriggerTestBehavior unit = (TriggerTestBehavior)Instantiate(dummyUnit, Vector3.zero, Quaternion.identity);

		tower.fixTermination = true;
		unit.fixTermination = true;

		yield return new WaitForFixedUpdate();

		unit.gameObject.SetActiveRecursively(false);

		yield return new WaitForFixedUpdate();

		if(AssertFlagStates(4, true, true, true, true)) { yield break; }

		Destroy(tower.gameObject);
		Destroy(unit.gameObject);
	}
}
