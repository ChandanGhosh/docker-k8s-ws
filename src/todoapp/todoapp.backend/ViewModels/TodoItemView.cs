using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoapp.backend.Models;

namespace todoapp.backend.ViewModels
{
    public class TodoItemView
    {
        public TodoItemView(TodoItem item, HttpRequest req)
        {
            Title = item.Title;
            Completed = item.Completed ?? false;
            Url = req.Scheme + "://" + req.Host + "/" + item.Id;
            Order = item.Order;
        }

        public string Title { get; }
        public bool Completed { get; }
        public string Url { get; }
        public int? Order { get; }
    }
}
