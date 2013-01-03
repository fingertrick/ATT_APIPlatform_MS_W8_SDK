// <copyright file="ArgumentTests.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ATT.Utility.Tests
{
	[TestClass]
	public class ArgumentTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentPassedNullExpressionShouldThrowException()
		{
			string someParam = null;
			Argument.ExpectNotNull(() => someParam);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentPassedEmptyStringExpressionShouldThrowException()
		{
			var someParam = string.Empty;
			Argument.ExpectNotNullOrWhiteSpace(() => someParam);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentPassedNullStringExpressionShouldThrowException()
		{
			string someParam = null;
			Argument.ExpectNotNullOrWhiteSpace(() => someParam);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentPassedWhiteSpaceStringExpressionShouldThrowException()
		{
			var someParam = "   ";
			Argument.ExpectNotNullOrWhiteSpace(() => someParam);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ArgumentPassedEmptyGuidStringExpressionShouldThrowException()
		{
			var someParam = Guid.Empty;
			Argument.ExpectNotEmptyGuid(() => someParam);
		}

		[TestMethod]
		public void ArgumentPassedNotEmptyStringExpressionShouldNotThrowException()
		{
			var notEmptyString = "aa";
			Argument.ExpectNotNullOrWhiteSpace(() => notEmptyString);
		}

		[TestMethod]
		public void ArgumentPassedNotNullObjectExpressionShouldNotThrowException()
		{
			var notNullObject = new Object();
			Argument.ExpectNotNull(() => notNullObject);
		}

		[TestMethod]
		public void ArgumentPassedNotEmptyGuidExpressionShouldNotThrowException()
		{
			var notEmptyGuid = Guid.NewGuid();
			Argument.ExpectNotEmptyGuid(() => notEmptyGuid);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ArgumentPassedFalseConditionShouldThrowException()
		{
			Argument.Expect(() => 1 != 1, "someParam", "test message");
		}

		[TestMethod]
		public void ArgumentPassedTrueConditionShouldNotThrowException()
		{
			Argument.Expect(() => 1 != 2, "someParam", "test message");
		}
	}
}
