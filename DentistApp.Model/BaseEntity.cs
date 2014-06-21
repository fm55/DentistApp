using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;

namespace DentistApp.DAL
{
    public interface IEntity
    {
        EntityState EntityState { get; set; }
    }

    public enum EntityState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }

    public abstract class BaseEntity : IEntity
    {
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public User CurrentUser { get; set; }
        public virtual List<Note> Notes { get; set; }

        public BaseEntity()
        {
            DateCreated = DateTime.Now;
            IsDeleted = false;
            CurrentUser = UserToken.CurrentUser;
            Notes = new List<Note>();
        }

        public EntityState EntityState
        {
            get;
            set;
        }
    }
}
