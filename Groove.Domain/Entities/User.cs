using System;
using System.Collections.Generic;
using Utilities.Domain;

namespace Groove.Domain.Entities
{
	[Serializable]
	public class User : DomainEntity
	{
		#region Constructors
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public User()
		{
			_username = "";
			_friendlyName = "";
			_emailAddress = "";
			_inActive = false;
			_deleted = false;
			_userGroups = new List<UserGroup>();
		}
		#endregion

		#region Public Properties
		private string _username;
		/// <summary>
		/// Unique username to identify this user instance.  Normally DOMAIN\Username.
		/// The SetUsername helper method should be used to set this property value.
		/// </summary>
		[DomainSignature]
		public virtual string Username { get { return _username; } protected set { _username = value; } }
		private string _friendlyName;
		/// <summary>
		/// Friendly display name for this user.  Initially set to Username.
		/// </summary>
		public virtual string FriendlyName { get { return _friendlyName; } set { _friendlyName = value; } }
		private string _emailAddress;
		/// <summary>
		/// Email address for this user.
		/// </summary>
		public virtual string EmailAddress { get { return _emailAddress; } set { _emailAddress = value; } }
		private bool _inActive;
		/// <summary>
		/// Is this user active, and allowed to use the system?
		/// </summary>
		public virtual bool InActive { get { return _inActive; } set { _inActive = value; } }
		private bool _deleted;
		/// <summary>
		/// Has this user been marked as deleted?
		/// </summary>
		public virtual bool Deleted { get { return _deleted; } set { _deleted = value; } }
		private IList<UserGroup> _userGroups;
		/// <summary>
		/// UserGroups associated with this user instance.  The UserGroups should
		/// collection should be modified with the AddUserGroup and RemoveUserGroup helpers
		/// to maintain either side of the object relationship.
		/// </summary>
		public virtual IList<UserGroup> UserGroups { get { return _userGroups; } protected set { _userGroups = value; } }

		#endregion

		#region Collection Property Helpers
		/// <summary>
		/// ??
		/// </summary>
		/// <param name="userGroup"></param>
		public void AddUserGroup(UserGroup userGroup)
		{
			if (!userGroup.Users.Contains(this))
			{
				userGroup.Users.Add(this);
			}
			if (!UserGroups.Contains(userGroup))
			{
				UserGroups.Add(userGroup);
			}
		}

		/// <summary>
		/// ??
		/// </summary>
		/// <param name="userGroup"></param>
		public void RemoveUserGroup(UserGroup userGroup)
		{
			if (userGroup.Users.Contains(this))
			{
				userGroup.Users.Remove(this);
			}
			if (UserGroups.Contains(userGroup))
			{
				UserGroups.Remove(userGroup);
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// ??
		/// </summary>
		/// <param name="username"></param>
		public void SetUsername(string username)
		{
			if (String.IsNullOrEmpty(FriendlyName))
			{
				FriendlyName = username;
			}
			Username = username;
		}
		#endregion
	}
}
