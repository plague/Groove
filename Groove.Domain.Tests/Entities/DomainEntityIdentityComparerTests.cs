using System;
using Groove.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Groove.Domain.Tests.Entities
{
	/// <summary>
	/// DomainEntityIdentityComparer Tests.
	/// </summary>
	[TestClass]
	public class DomainEntityIdentityComparerTests
	{
		#region DomainEntityIdentityComparer Test Stub Classes
		/// <summary>
		/// A testing stub for the DomainEntityIdentityComparer class.
		/// </summary>
		/// <typeparam name="T">The type of the DomainEntityIdentityComparerStub property.</typeparam>
		private sealed class DomainEntityIdentityComparerStub<T>
		{
			#region Constructors
			/// <summary>
			/// Default Constructor.
			/// </summary>
			public DomainEntityIdentityComparerStub()
			{
				Property = default(T);
			}
			#endregion

			#region Public Properties
			/// <summary>
			/// The typed property to use for the comparision and equality testing.
			/// </summary>
			public T Property { get; set; }
			#endregion
		}

		/// <summary>
		/// A testing stub for validating the DomainEntityIdentityComparer class.
		/// </summary>
		private sealed class SomeClass
		{
			#region Constructors
			/// <summary>
			/// Default Constructor.
			/// </summary>
			public SomeClass()
			{
				Property = 1;
				GetProperty();
			}
			#endregion

			#region Public Properties
			/// <summary>
			/// A Property for SomeClass.
			/// </summary>
			private int Property { get; set; }
			#endregion

			#region Private Methods
			/// <summary>
			/// Dummy Method on Class.
			/// </summary>
			private void GetProperty()
			{
				if (Property == default(Int32))
				{
					Property = 1;
				}
			}
			#endregion
		}
		#endregion

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		#region Comparison and Equality Tests
		/// <summary>
		/// Validate that the IsNullOrDefault method of DomainEntityIdentityComparer
		/// returns true when checking against the default value of an integer when the
		/// value assigned is null or default.
		/// </summary>
		[TestMethod]
		public void DomainEntityIdentityComparer_IsNullOrDefault_Returns_True_When_Integer_Value_Is_Default()
		{
			// Arrange
			var o = new DomainEntityIdentityComparerStub<Int32> { Property = default(Int32) };

			// Act

			// Assert
			Assert.AreEqual(default(Int32), o.Property);
			Assert.IsTrue(o.Property.IsNullOrDefault());
		}

		/// <summary>
		/// Validate that the IsNullOrDefault method of DomainEntityIdentityComparer
		/// returns false when checking against the default value of an integer when the
		/// value assigned is not null or default.
		/// </summary>
		[TestMethod]
		public void DomainEntityIdentityComparer_IsNullOrDefault_Returns_False_When_Integer_Value_Is_Not_Default()
		{
			// Arrange
			var o = new DomainEntityIdentityComparerStub<Int32> { Property = 1 };

			// Act

			// Assert
			Assert.AreNotEqual(default(Int32), o.Property);
			Assert.AreEqual(1, o.Property);
			Assert.IsFalse(o.Property.IsNullOrDefault());
		}

		/// <summary>
		/// Validate that the IsNullOrDefault method of DomainEntityIdentityComparer
		/// returns true when checking against the default value of a string when the
		/// value assigned is null or default.
		/// </summary>
		[TestMethod]
		public void DomainEntityIdentityComparer_IsNullOrDefault_Returns_True_When_String_Value_Is_Null()
		{
			// Arrange
			var o = new DomainEntityIdentityComparerStub<String> { Property = default(String) };

			// Act

			// Assert
			Assert.AreEqual(default(String), o.Property);
			Assert.IsTrue(o.Property.IsNullOrDefault());
		}

		/// <summary>
		/// Validate that the IsNullOrDefault method of DomainEntityIdentityComparer
		/// returns true when checking against the default value of a string when the
		/// value assigned is an empty string.
		/// </summary>
		[TestMethod]
		public void DomainEntityIdentityComparer_IsNullOrDefault_Returns_True_When_String_Value_Is_Empty_String()
		{
			// Arrange
			var o = new DomainEntityIdentityComparerStub<String> { Property = "" };

			// Act

			// Assert
			Assert.AreNotEqual(default(String), o.Property);
			Assert.AreEqual("", o.Property);
			Assert.IsTrue(o.Property.IsNullOrDefault());
		}

		/// <summary>
		/// Validate that the IsNullOrDefault method of DomainEntityIdentityComparer
		/// returns false when checking against the default value of a string when the
		/// value assigned is not null or default or an empty string.
		/// </summary>
		[TestMethod]
		public void DomainEntityIdentityComparer_IsNullOrDefault_Returns_False_When_String_Value_Is_Not_Null_Or_Empty_String()
		{
			// Arrange
			var o = new DomainEntityIdentityComparerStub<String> { Property = "Some Value" };

			// Act

			// Assert
			Assert.AreNotEqual(default(String), o.Property);
			Assert.AreEqual("Some Value", o.Property);
			Assert.IsFalse(o.Property.IsNullOrDefault());
		}

		/// <summary>
		/// Validate that the IsNullOrDefault method of DomainEntityIdentityComparer
		/// returns true when checking against the default value of a reference value object
		/// (class) when the value assigned is null or default.
		/// </summary>
		[TestMethod]
		public void DomainEntityIdentityComparer_IsNullOrDefault_Returns_True_When_Reference_Value_Object_Is_Null()
		{
			// Arrange
			var o = new DomainEntityIdentityComparerStub<SomeClass> { Property = null };

			// Act

			// Assert
			Assert.AreEqual(default(SomeClass), o.Property);
			Assert.IsNull(o.Property);
			Assert.IsTrue(o.Property.IsNullOrDefault());
		}

		/// <summary>
		/// Validate that the IsNullOrDefault method of DomainEntityIdentityComparer
		/// returns false when checking against the default value of a reference value object
		/// (class) when the value assigned is not null or default.
		/// </summary>
		[TestMethod]
		public void DomainEntityIdentityComparer_IsNullOrDefault_Returns_False_When_Reference_Value_Object_Is_Not_Null()
		{
			// Arrange
			var o = new DomainEntityIdentityComparerStub<SomeClass> { Property = new SomeClass() };

			// Act

			// Assert
			Assert.AreNotEqual(default(SomeClass), o.Property);
			Assert.IsNotNull(o.Property);
			Assert.IsFalse(o.Property.IsNullOrDefault());
		}
		#endregion
	}
}
