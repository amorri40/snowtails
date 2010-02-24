//-----------------------------------------------------------------
//  TestSomethingElse v0.1 (2009-07-04)
//  Copyright 2009 MrJoy, Inc.
//  All rights reserved
//
//  2009-07-04 - jdf - Initial version.
//
//-----------------------------------------------------------------
// Example test suite just so we have multiple suites (to demo 
// UnityTestController) and have something producing errors.
//-----------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class TestSomethingElse : UnityTest {
	public IEnumerator TestThatShouldPass() {
		if(AssertTrue(true, "Meh")) { yield break; }
		yield return 0;
	}

	public IEnumerator TestThatShouldFail() {
		if(Fail("This was expected.")) { yield break; }
	}
}
