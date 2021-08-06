using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapp.backend.Models
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool? Completed { get; set; }
        public int? Order { get; set; }

        public bool ShouldSerializeId() => false;
        public bool ShouldDeserializeId() => true;
    }
}
