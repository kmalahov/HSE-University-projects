using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ToDoApi2.Models
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItem> TodoItems { get; set; }
    }
}