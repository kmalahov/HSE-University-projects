using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoApi2.Models
{
    public class ToDoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

    }
}