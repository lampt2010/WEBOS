//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace onsoft.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupLibrary
    {
        public GroupLibrary()
        {
            this.Libraries = new HashSet<Library>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Ord { get; set; }
        public string Lang { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
    
        public virtual ICollection<Library> Libraries { get; set; }
    }
}