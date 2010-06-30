using System;
using System.Linq;
using Groove.Domain.Attributes;
using Groove.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Groove.Domain.Tests.Entities
{
	/// <summary>
	/// DomainEntity base class tests.
	/// </summary>
	[TestClass]
	public class DomainEntityTests
	{
		#region DomainEntity Test Stub Classes
		/// <summary>
		/// A testing stub class for DomainEntity.  Allows checking of the DomainEntity methods.
		/// </summary>
		/// <typeparam name="T">The type of the DomainEntity identifier.</typeparam>
		private sealed class DomainEntityStub<T> : DomainEntity<T>
		{
			#region Constructors
			/// <summary>
			/// Default constructor.
			/// </summary>
			public DomainEntityStub()
			{
				_property1 = "";
				_property2 = "";
			}
			
#pragma warning disable 612,618
			/// <summary>
			/// Constructor allowing the DomainEntity identifier to be specified.  This constructor
			/// is only present to facilitate testing, and should not be used in production code.
			/// </summary>
			/// <param name="id">DomainEntity identifier.</param>
			public DomainEntityStub(T id) : base(id)
#pragma warning restore 612,618
			{
				_property1 = "";
				_property2 = "";
			}
			#endregion

			#region Public Properties
			private string _property1;
			/// <summary>
			/// A Property who's value contributes to this entities business signature.
			/// </summary>
			[DomainSignature]
// ReSharper disable ConvertToAutoProperty
			public string Property1 { get { return _property1; } set { _property1 = value; } }
// ReSharper restore ConvertToAutoProperty
			private string _property2;
			/// <summary>
			/// A property which does not contribute to this entities business signature.
			/// </summary>
// ReSharper disable ConvertToAutoProperty
			public string Property2 { get { return _property2; } set { _property2 = value; } }
// ReSharper restore ConvertToAutoProperty
			#endregion
		}

		/// <summary>
		/// A testing stub class for DomainEntity.  Allows checking of the DomainEntity methods.
		/// </summary>
		/// <typeparam name="T">The type of the DomainEntity identifier.</typeparam>
		private sealed class DomainEntityStub2<T> : DomainEntity<T>
		{
			#region Constructors
			/// <summary>
			/// Default constructor.
			/// </summary>
			public DomainEntityStub2()
			{
				_property1 = "";
				_property2 = "";
			}

#pragma warning disable 612,618
			/// <summary>
			/// Constructor allowing the DomainEntity identifier to be specified.  This constructor
			/// is only present to facilitate testing, and should not be used in production code.
			/// </summary>
			/// <param name="id">DomainEntity identifier.</param>
			public DomainEntityStub2(T id)
				: base(id)
#pragma warning restore 612,618
			{
				_property1 = "";
				_property2 = "";
			}
			#endregion

			#region Public Properties
			private string _property1;
			/// <summary>
			/// A Property who's value contributes to this entities business signature.
			/// </summary>
			[DomainSignature]
			// ReSharper disable ConvertToAutoProperty
			public string Property1 { get { return _property1; } set { _property1 = value; } }
			// ReSharper restore ConvertToAutoProperty
			private string _property2;
			/// <summary>
			/// A property which does not contribute to this entities business signature.
			/// </summary>
			// ReSharper disable ConvertToAutoProperty
			public string Property2 { get { return _property2; } set { _property2 = value; } }
			// ReSharper restore ConvertToAutoProperty
			#endregion
		}
		#endregion

		#region Initialisation Tests
		/// <summary>
		/// Validate that a DomainEntity is initialised with the expected values.
		/// </summary>
		[TestMethod]
		public void DomainEntity_Is_Initialised_With_Expected_Values()
		{
			// Arrange
			var entity = new DomainEntityStub<Int32>();

			// Act

			// Assert
			Assert.AreEqual(entity.Id, default(Int32));
			Assert.IsNull(entity.RowTimestamp);
			// Not strictly necessary, but might as well double check while we are here...
			Assert.AreEqual("", entity.Property1);
			Assert.AreEqual("", entity.Property2);
		}

		/// <summary>
		/// Validate that a DomainEntity with the default initialised value for the
		/// identity (Id) property is classified as Transient.
		/// </summary>
		[TestMethod]
		public void DomainEntity_With_Default_Value_Identifier_Is_Transient()
		{
			// Arrange
			var entity = new DomainEntityStub<Int32>();

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity.Id);
			Assert.AreEqual("", entity.Property1);
			Assert.AreEqual("", entity.Property2);
			Assert.IsTrue(entity.IsTransient());
		}

		/// <summary>
		/// Validate that a DomainEntity with a non-default value for the
		/// identity (Id) property is not classified as Transient.
		/// </summary>
		[TestMethod]
		public void DomainEntity_With_Non_Default_Identifier_Is_Not_Transient()
		{
			// Arrange
			var entity = new DomainEntityStub<Int32>(1);

			// Act

			//Assert
			Assert.AreEqual(1, entity.Id);
			Assert.AreEqual("", entity.Property1);
			Assert.AreEqual("", entity.Property2);
			Assert.IsFalse(entity.IsTransient());
		}
		#endregion

		#region Business Signature Tests
		/// <summary>
		/// Validate that a DomainEntity with properties marked as being part of
		/// the entities business signature are returned in the business signature
		/// properties collection as expected.
		/// </summary>
		[TestMethod]
		public void DomainEntity_Has_Expected_Business_Signature_Properties()
		{
			// Arrange
			var entity = new DomainEntityStub<Int32>();

			// Act
			var signatureProperties = entity.GetSignatureProperties();
			var signatureProperty = entity.GetType().GetProperties()
				.Where(p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true) && p.Name == "Property1").First();
			
			// Assert
			Assert.AreEqual(default(Int32), entity.Id);
			Assert.AreEqual("", entity.Property1);
			Assert.AreEqual("", entity.Property2);
			Assert.AreEqual(1, signatureProperties.ToList().Count);
			Assert.AreEqual(signatureProperty, signatureProperties.First());
		}

		/// <summary>
		/// Validate the a DomainObject with business signature properties rutns the
		/// expected business signature.
		/// </summary>
		[TestMethod]
		public void DomainEntity_Has_Expected_Business_Signature()
		{
			// Arrange
			var entity = new DomainEntityStub<Int32> {Property1 = "", Property2 = ""};

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity.Id);
			Assert.AreEqual("", entity.Property1);
			Assert.AreEqual("", entity.Property2);
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:, IsTransient:True)", entity.ToString());
		}

		/// <summary>
		/// Validate that an DomainEntity objects instances business signature changes
		/// when a property marked as being part of the business signature changes.
		/// </summary>
		[TestMethod]
		public void DomainEntity_Business_Signature_Changes_When_Signature_Property_Values_Change()
		{
			// Arrange
			var entity = new DomainEntityStub<Int32> { Property1 = "Initial", Property2 = "Initial" };

			// Act
			var initialSignature = entity.ToString();
			entity.Property1 = "Changed";
			var changedSignature = entity.ToString();

			// Assert
			Assert.AreNotEqual(initialSignature, changedSignature);
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Initial, IsTransient:True)", initialSignature);
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Changed, IsTransient:True)", changedSignature);
		}

		/// <summary>
		/// Validate that an DomainEntity objects instances business signature does
		/// not change when a property not  marked as being part of the business signature
		/// changes. 
		/// </summary>
		[TestMethod]
		public void DomainEntity_Business_Signature_Does_Not_Change_When_Non_Signature_Property_Values_Change()
		{
			// Arrange
			var entity = new DomainEntityStub<Int32> { Property1 = "Initial", Property2 = "Initial" };

			// Act
			var initialSignature = entity.ToString();
			entity.Property2 = "Changed";
			var changedSignature = entity.ToString();

			// Assert
			Assert.AreEqual(initialSignature, changedSignature);
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Initial, IsTransient:True)", initialSignature);
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Initial, IsTransient:True)", changedSignature);
		}
		#endregion

		#region Equality Tests
		/// <summary>
		/// Validate that two DomainEntity objects return the same Hashcode value when both
		/// object instances are not transient and have the same identifier value and are of
		/// the same type.
		/// </summary>
		[TestMethod]
		public void DomainEntities_Have_The_Same_Hashcode_If_They_Are_Not_Transient_And_Have_The_Same_Identifier()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32>(1);
			var entity2 = new DomainEntityStub<Int32>(1);

			// Act

			// Assert
			Assert.AreEqual(1, entity1.Id);
			Assert.AreEqual("", entity1.Property1);
			Assert.AreEqual("", entity1.Property2);
			Assert.IsFalse(entity1.IsTransient());
			Assert.AreEqual(1, entity2.Id);
			Assert.AreEqual("", entity2.Property1);
			Assert.AreEqual("", entity2.Property2);
			Assert.IsFalse(entity2.IsTransient());
			Assert.AreEqual(entity1.GetHashCode(), entity2.GetHashCode());
		}

		[TestMethod]
		public void DomainEntities_Do_Not_Have_The_Same_Hashcode_If_They_Are_Not_Transient_And_Have_Different_Identifiers()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32>(1);
			var entity2 = new DomainEntityStub<Int32>(2);

			// Act

			// Assert
			Assert.AreEqual(1, entity1.Id);
			Assert.AreEqual("", entity1.Property1);
			Assert.AreEqual("", entity1.Property2);
			Assert.IsFalse(entity1.IsTransient());
			Assert.AreEqual(2, entity2.Id);
			Assert.AreEqual("", entity2.Property1);
			Assert.AreEqual("", entity2.Property2);
			Assert.IsFalse(entity2.IsTransient());
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode());
		}

		[TestMethod]
		public void DomainEntities_Have_The_Same_Hashcode_If_They_Are_Transient_And_Have_The_Same_Business_Signature()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32> { Property1 = "A Value" };
			var entity2 = new DomainEntityStub<Int32> { Property1 = "A Value" };

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity1.Id);
			Assert.AreEqual("A Value", entity1.Property1);
			Assert.AreEqual("", entity1.Property2);
			Assert.IsTrue(entity1.IsTransient());
			Assert.AreEqual(default(Int32), entity2.Id);
			Assert.AreEqual("A Value", entity2.Property1);
			Assert.AreEqual("", entity2.Property2);
			Assert.IsTrue(entity2.IsTransient());
			Assert.AreEqual(entity1.GetHashCode(), entity2.GetHashCode());
		}

		[TestMethod]
		public void DomainEntities_Do_Not_Have_The_Same_Hashcode_If_They_Are_Transient_And_Have_Different_Business_Signatures()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32> { Property1 = "A Value"};
			var entity2 = new DomainEntityStub<Int32> { Property1 = "A Different Value" };

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity1.Id);
			Assert.AreEqual("A Value", entity1.Property1);
			Assert.AreEqual("", entity1.Property2);
			Assert.IsTrue(entity1.IsTransient());
			Assert.AreEqual(default(Int32), entity2.Id);
			Assert.AreEqual("A Different Value", entity2.Property1);
			Assert.AreEqual("", entity2.Property2);
			Assert.IsTrue(entity2.IsTransient());
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode());
		}

		[TestMethod]
		public void DomainEntities_Do_Not_Have_The_Same_Hascode_If_They_Are_Not_Of_The_Same_Type_And_Transient()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32> { Property1 = "Property1", Property2 = "Property2" };
			var entity2 = new DomainEntityStub2<Int32> { Property1 = "Property1", Property2 = "Property2" };

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity1.Id);
			Assert.AreEqual("Property1", entity1.Property1);
			Assert.AreEqual("Property2", entity1.Property2);
			Assert.IsTrue(entity1.IsTransient());
			Assert.AreEqual(default(Int32), entity2.Id);
			Assert.AreEqual("Property1", entity2.Property1);
			Assert.AreEqual("Property2", entity2.Property2);
			Assert.IsTrue(entity2.IsTransient());
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode());
		}

		[TestMethod]
		public void DomainEntities_Do_Not_Have_The_Same_Hascode_If_They_Are_Not_Of_The_Same_Type_Even_With_The_Same_Identifier()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32>(1);
			var entity2 = new DomainEntityStub2<Int32>(1);

			// Act

			// Assert
			Assert.AreEqual(1, entity1.Id);
			Assert.AreEqual("", entity1.Property1);
			Assert.AreEqual("", entity1.Property2);
			Assert.IsFalse(entity1.IsTransient());
			Assert.AreEqual(1, entity2.Id);
			Assert.AreEqual("", entity2.Property1);
			Assert.AreEqual("", entity2.Property2);
			Assert.IsFalse(entity2.IsTransient());
			Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode());
		}

		[TestMethod]
		public void DomainEntities_Are_Equal_If_They_Are_Not_Transient_And_Have_The_Same_Identifier()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32>(1);
			var entity2 = new DomainEntityStub<Int32>(1);

			// Act

			// Assert
			Assert.AreEqual(1, entity1.Id);
			Assert.AreEqual("", entity1.Property1);
			Assert.AreEqual("", entity1.Property2);
			Assert.IsFalse(entity1.IsTransient());
			Assert.AreEqual(1, entity2.Id);
			Assert.AreEqual("", entity2.Property1);
			Assert.AreEqual("", entity2.Property2);
			Assert.IsFalse(entity2.IsTransient());
			Assert.IsTrue(entity1.Equals(entity2));
		}

		[TestMethod]
		public void DomainEntities_Are_Not_Equal_If_They_Are_Not_Transient_And_Have_Different_Identifiers()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32>(1);
			var entity2 = new DomainEntityStub<Int32>(2);

			// Act

			// Assert
			Assert.AreEqual(1, entity1.Id);
			Assert.IsFalse(entity1.IsTransient());
			Assert.AreEqual(2, entity2.Id);
			Assert.IsFalse(entity2.IsTransient());
			Assert.IsFalse(entity1.Equals(entity2));
		}

		[TestMethod]
		public void DomainEntities_Are_Equal_If_They_Are_Transient_And_Have_The_Same_Business_Signature()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32> { Property1 = "Property1", Property2 = "Property2" };
			var entity2 = new DomainEntityStub<Int32> { Property1 = "Property1", Property2 = "Property2" };

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity1.Id);
			Assert.AreEqual("Property1", entity1.Property1);
			Assert.AreEqual("Property2", entity1.Property2);
			Assert.IsTrue(entity1.IsTransient());
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Property1, IsTransient:True)", entity1.ToString());
			Assert.AreEqual(default(Int32), entity2.Id);
			Assert.AreEqual("Property1", entity2.Property1);
			Assert.AreEqual("Property2", entity2.Property2);
			Assert.IsTrue(entity2.IsTransient());
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Property1, IsTransient:True)", entity2.ToString());
			Assert.IsTrue(entity1.Equals(entity2));
		}

		[TestMethod]
		public void DomainEntities_Are_Not_Equal_If_They_Are_Transient_And_Have_Different_Business_Signatures()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32> { Property1 = "Property1", Property2 = "Property2" };
			var entity2 = new DomainEntityStub<Int32> { Property1 = "Different Property1", Property2 = "Property2" };

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity1.Id);
			Assert.AreEqual("Property1", entity1.Property1);
			Assert.AreEqual("Property2", entity1.Property2);
			Assert.IsTrue(entity1.IsTransient());
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Property1, IsTransient:True)", entity1.ToString());
			Assert.AreEqual(default(Int32), entity2.Id);
			Assert.AreEqual("Different Property1", entity2.Property1);
			Assert.AreEqual("Property2", entity2.Property2);
			Assert.IsTrue(entity2.IsTransient());
			Assert.AreEqual("DomainEntityStub`1: [Id:0] (Property1:Different Property1, IsTransient:True)", entity2.ToString());
			Assert.IsFalse(entity1.Equals(entity2));
		}

		[TestMethod]
		public void DomainEntities_Are_Not_Equal_If_They_Are_Not_Of_The_Same_Type_And_Transient()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32> { Property1 = "Property1", Property2 = "Property2" };
			var entity2 = new DomainEntityStub2<Int32> { Property1 = "Property1", Property2 = "Property2" };

			// Act

			// Assert
			Assert.AreEqual(default(Int32), entity1.Id);
			Assert.AreEqual("Property1", entity1.Property1);
			Assert.AreEqual("Property2", entity1.Property2);
			Assert.IsTrue(entity1.IsTransient());
			Assert.AreEqual(default(Int32), entity2.Id);
			Assert.AreEqual("Property1", entity2.Property1);
			Assert.AreEqual("Property2", entity2.Property2);
			Assert.IsTrue(entity2.IsTransient());
			Assert.IsFalse(entity1.Equals(entity2));
		}

		[TestMethod]
		public void DomainEntities_Are_Not_Equal_If_They_Are_Not_Of_The_Same_Type_Even_With_The_Same_Identifier()
		{
			// Arrange
			var entity1 = new DomainEntityStub<Int32>(1);
			var entity2 = new DomainEntityStub2<Int32>(1);

			// Act

			// Assert
			Assert.AreEqual(1, entity1.Id);
			Assert.AreEqual("", entity1.Property1);
			Assert.AreEqual("", entity1.Property2);
			Assert.IsFalse(entity1.IsTransient());
			Assert.AreEqual(1, entity2.Id);
			Assert.AreEqual("", entity2.Property1);
			Assert.AreEqual("", entity2.Property2);
			Assert.IsFalse(entity2.IsTransient());
			Assert.IsFalse(entity1.Equals(entity2));
		}
		#endregion
	}
}
