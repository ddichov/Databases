//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UsersAndGroups.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Group
    {
        public Group()
        {
            this.Users = new HashSet<User>();
        }
    
        public int groupId { get; set; }
        public string groupName { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}
