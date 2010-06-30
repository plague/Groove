using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using Groove.Domain.Attributes;

namespace Groove.Domain.Entities
{
	/// <summary>
	/// Provides a standard base class for facilitating comparison of objects.
	/// </summary>
	/// <typeparam name="T">The key type.</typeparam>
	[Serializable]
	public abstract class DomainEntity<T>
	{
		#region Data Members
		/// <summary>
		/// To help ensure hashcode uniqueness, a carefully selected random number multiplier 
		/// is used within the calculation.  Goodrich and Tamassia's Data Structures and
		/// Algorithms in Java asserts that 31, 33, 37, 39 and 41 will produce the fewest number
		/// of collissions.  See http://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
		/// for more information.
		/// </summary>
		protected const int HashMultiplier = 31;
		/// <summary>
		/// Provides a caching mechanism for objects hashcode.
		/// </summary>
		private int? _cachedHashcode;
		/// <summary>
		/// Dictionary of signature properties.
		/// </summary>
		[ThreadStatic]
		private static IDictionary<Type, IEnumerable<PropertyInfo>> _signaturePropertiesDictionary;
		/// <summary>
		/// Provides a caching mechanism for objects signature properties string.
		/// </summary>
		private string _cachedSignaturePropertiesString;
		/// <summary>
		/// Provides a simple caching mechanism for the last known values of the signature properties.
		/// </summary>
		private IDictionary<string, string> _signatureValuesCache;
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor.
		/// </summary>
		protected DomainEntity()
		{
			_id = default(T);
			_rowTimestamp = null;
			_cachedHashcode = null;
			_signatureValuesCache = null;
			_cachedSignaturePropertiesString = "";
		}

		/// <summary>
		/// Constructor allowing the DomainEntity identifier to be specified.  This constructor
		/// is only present to facilitate testing, and should not be used in production code.
		/// </summary>
		/// <param name="id">DomainEntity identifier.</param>
		[Obsolete("This constructor should only be used for testing purposes.")]
		protected DomainEntity(T id)
		{
			_id = id;
			_rowTimestamp = null;
			_cachedHashcode = null;
			_signatureValuesCache = null;
			_cachedSignaturePropertiesString = "";
		}

		/// <summary>
		/// Default Static Constructor.
		/// </summary>
		static DomainEntity()
		{
			_signaturePropertiesDictionary = null;
		}
		#endregion

		#region Public Properties
		private T _id;
		/// <summary>
		/// Domain objects database identifier.
		/// </summary>
		public virtual T Id { get { return _id; } protected set { _id = value; } }
		private byte[] _rowTimestamp;
		/// <summary>
		/// Change tracking property for persistence.
		/// </summary>
		public virtual byte[] RowTimestamp { get { return _rowTimestamp; } protected set { _rowTimestamp = value; } }
		#endregion

		#region Private Methods
		/// <summary>
		/// Determines whether the specified DomainEntity.Id is
		/// the same, and not the default value as the current
		/// DomainEntity.Id.
		/// </summary>
		/// <param name="compareTo">The DomainEntity to compare Ids to.
		/// </param>
		/// <returns>true if the specified DomainEntity.Id is equal to
		/// the current DomainEntity.Id; otherwise, false.</returns>
		private bool HasSameNoneDefaultIdAs(DomainEntity<T> compareTo)
		{
			return (!Id.IsNullOrDefault() && !compareTo.Id.IsNullOrDefault()) && Id.Equals(compareTo.Id)
				&& IsOfSameType(compareTo);
		}

		/// <summary>
		/// Compares object hash codes, which should be calculated from business
		/// signature in derived classes.
		/// </summary>
		/// <param name="compareTo">The DomainObject to compare
		/// Business Signature to.</param>
		/// <returns>true if the specified DomainObject Business
		/// Signature is equal to the current DomainEntity Business
		/// Signature; otherwise, false.</returns>
		private bool HasSameBusinessSignatureAs(DomainEntity<T> compareTo)
		{
			var signatureProperties = GetSignatureProperties();

			if ((from property in signatureProperties
			     let valueOfThisObject = property.GetValue(this, null)
			     let valueToCompareTo = property.GetValue(compareTo, null)
			     where valueOfThisObject != null || valueToCompareTo != null
			     where (valueOfThisObject == null ^ valueToCompareTo == null) || (!valueOfThisObject.Equals(valueToCompareTo))
			     select valueOfThisObject).Any())
			{
				return false;
			}

			// If we've gotten this far and signature properties were found, then we can
			// assume that everything matched; otherwise, if there were no signature 
			// properties, then simply return the default bahavior of Equals
			return signatureProperties.Any() || base.Equals(compareTo);
		}

		/// <summary>
		/// Checks that an DomainEntity is of the same type as the current DomainEntity instance.
		/// </summary>
		/// <param name="compareTo">The DomainEntity to compare types.</param>
		/// <returns>true if DomainEntities are of the same type; otherwise, false.</returns>
		private bool IsOfSameType(DomainEntity<T> compareTo)
		{
			return GetTypeUnproxied() == compareTo.GetTypeUnproxied();
		}

		/// <summary>
		/// Gets the signature propities section of the ToString() method value.
		/// </summary>
		/// <returns>A string representation instance of the signature properties.</returns>
		private string SignaturePropertiesString()
		{
			var signatureProperties = GetSignatureProperties();

			if (signatureProperties.ToList().Count == 0)
			{
				_cachedSignaturePropertiesString = "";
				return _cachedSignaturePropertiesString;
			}

			var signatureChanged = HasSignatureChanged(signatureProperties);
			if (String.IsNullOrEmpty(_cachedSignaturePropertiesString) || signatureChanged)
			{
				var sb = new StringBuilder("");

				foreach (var property in signatureProperties)
				{
					var value = property.GetValue(this, null);

					if (value != null)
					{
						sb.AppendFormat("{0}:{1}, ", property.Name, value);
					}
				}

				_cachedSignaturePropertiesString = sb.ToString();
			}

			return _cachedSignaturePropertiesString;
		}

		/// <summary>
		/// Checks whether the signature of the DomainEntity has changed.
		/// </summary>
		/// <param name="signatureProperties"></param>
		/// <returns>true if any signature value is cached and different to the current
		/// signature property value; otherwise, false.</returns>
		private bool HasSignatureChanged(IEnumerable<PropertyInfo> signatureProperties)
		{
			if (_signatureValuesCache == null)
			{
				_signatureValuesCache = new Dictionary<string, string>();

				foreach (var signatureProperty in signatureProperties)
				{
					_signatureValuesCache.Add(signatureProperty.Name, signatureProperty.GetValue(this, null).ToString());
				}

				return false;
			}

			return signatureProperties
				.Any(signatureProperty => _signatureValuesCache[signatureProperty.Name] != signatureProperty.GetValue(this, null).ToString());
		}
		#endregion

		#region Protected Methods
		/// <summary>
		/// When NHibernate proxies objects, it masks the type of the actual entity object.
		/// This wrapper burrows into the proxied object to get its actual type.
		/// 
		/// Although this assumes NHibernate is being used, it doesn't require any NHibernate
		/// related dependencies and has no bad side effects if NHibernate isn't being used.
		/// 
		/// Related discussion is at http://groups.google.com/group/sharp-architecture/browse_thread/thread/ddd05f9baede023a
		/// </summary>
		protected virtual Type GetTypeUnproxied()
		{
			return GetType();
		}

		/// <summary>
		/// Enforces the template method pattern to have child objects determine which specific 
		/// properties should and should not be included in the object signature comparison.
		/// Observe that the the BaseObject already takes care of performance caching, so this method 
		/// shouldn't worry about caching...just return the goods man!
		/// </summary>
		protected IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties()
		{
			return GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true));
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Transient objects are not associated with an item alreaqdy in storage.
		/// Usually means that the domain objects key value is equivalent to the default
		/// for the keys data type.
		/// </summary>
		/// <returns></returns>
		public bool IsTransient()
		{
			return Id.IsNullOrDefault();
		}

		/// <summary>
		/// Gets the DomainObjects properties which are marked as forming part of the business signature.
		/// </summary>
		public virtual IEnumerable<PropertyInfo> GetSignatureProperties()
		{
			IEnumerable<PropertyInfo> properties;

			if (_signaturePropertiesDictionary == null)
			{
				_signaturePropertiesDictionary = new Dictionary<Type, IEnumerable<PropertyInfo>>();
			}

			if (_signaturePropertiesDictionary.TryGetValue(GetType(), out properties))
			{
				return properties;
			}

			return (_signaturePropertiesDictionary[GetType()] = GetTypeSpecificSignatureProperties());
		}
		#endregion

		#region General Object Overrides
		/// <summary>
		/// Determines whether the specified DomainEntity is equal
		/// to the current DomainEntity.
		/// </summary>
		/// <param name="obj">The DomainEntity to compare with the
		/// current System.Object.</param>
		/// <returns>true if the specified System.Object is equal to the current
		/// DomainEntity; otherwise, false.</returns>
		public override sealed bool Equals(object obj)
		{
			var compareTo = obj as DomainEntity<T>;

			if (ReferenceEquals(this, compareTo))
			{
				return true;
			}

			return (compareTo != null) && (HasSameNoneDefaultIdAs(compareTo) ||
				// Sinces the ID's aren't the same, either of them must be transient to
				// compare the business value signatures.
				(IsOfSameType(compareTo) && (IsTransient() || compareTo.IsTransient())
				&& HasSameBusinessSignatureAs(compareTo)));
		}

		/// <summary>  
		/// Must be implemented to compare two objects, should be calculated from the
		/// business signature of any derived classes.  Serves as a hash function for
		/// a particular type.
		/// 
		/// Although it's necessary for NHibernate's use, this can 
		/// also be useful for business logic purposes and has been included in this base 
		/// class, accordingly.  Since it is recommended that GetHashCode change infrequently, 
		/// if at all, in an object's lifetime, it's important that properties are carefully
		/// selected which truly represent the signature of an object.
		/// </summary>
		/// <returns>A hash code for the current DomainEntity.
		/// </returns>
		public override sealed int GetHashCode()
		{
			if (IsTransient())
			{
				var signatureProperties = GetSignatureProperties();

				if (_cachedHashcode.HasValue && !HasSignatureChanged(signatureProperties))
				{
					return _cachedHashcode.Value;
				}

				unchecked
				{
					// It's possible for two objects to return the same hash code based on 
					// identically valued properties, even if they're of two different types, 
					// so we include the object's type in the hash calculation
					var hashCode = GetType().GetHashCode();

					hashCode = signatureProperties
						.Select(property => property.GetValue(this, null))
						.Where(value => value != null)
						.Aggregate(hashCode, (current, value) => (current*HashMultiplier) ^ value.GetHashCode());

					return signatureProperties.Any() ? hashCode : base.GetHashCode();

					// If no properties were flagged as being part of the signature of the object,
					// then simply return the hashcode of the base object as the hashcode.
				}
			}
			
			if (_cachedHashcode.HasValue)
			{
				return _cachedHashcode.Value;
			}

			unchecked
			{
				// It's possible for two objects to return the same hash code based on 
				// identically valued properties, even if they're of two different types, 
				// so we include the object's type in the hash calculation
				var hashCode = GetType().GetHashCode();
				_cachedHashcode = (hashCode * HashMultiplier) ^ Id.GetHashCode();
			}
			
			return _cachedHashcode.Value;
		}

		/// <summary>
		/// Returns a System.String that represents the current DomainEntity.
		/// </summary>
		/// <returns>A System.String instance that represents the current DomainEntity.
		/// </returns>
		public override sealed string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}: [Id:{1}] ({2}IsTransient:{3})", GetTypeUnproxied().Name, Id, SignaturePropertiesString(), IsTransient());
			return sb.ToString();
		}
		#endregion
	}
}
