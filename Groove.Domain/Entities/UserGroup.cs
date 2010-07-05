using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Domain;

namespace Groove.Domain.Entities
{
	public class UserGroup : DomainEntity
	{
		#region Constructors
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public UserGroup()
		{
			Name = "";
			Description = "";
			Locked = false;
			Users = new List<User>();
		}
		#endregion

		#region Public Properties
		[DomainSignature]
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		[DomainSignature]
		public virtual bool Locked { get; protected set; }
		/// <summary>
		/// UserGroups associated with this user instance.  The Users should
		/// collection should be modified with the AddUser and RemoveUserhelpers
		/// to maintain either side of the object relationship.
		/// </summary>
		public virtual IList<User> Users { get; protected set; }
		#endregion

		#region Collection Property Helpers
		public void AddUser(User user)
		{
			if (!user.UserGroups.Contains(this))
			{
				user.UserGroups.Add(this);
			}
			if (!Users.Contains(user))
			{
				Users.Add(user);
			}
		}

		public void RemoveUser(User user)
		{
			if (user.UserGroups.Contains(this))
			{
				user.UserGroups.Remove(this);
			}
			if (Users.Contains(user))
			{
				Users.Remove(user);
			}
		}
		#endregion
	}
}
