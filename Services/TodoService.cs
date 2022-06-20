using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Todo.Data;
using Todo.Models;

namespace Todo.Services
{
    public class TodoService
    {
        protected readonly TodoDbContext _dbcontext;

        public TodoService(TodoDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
       

        public List<TodoItem> GetAll()
        {
            return _dbcontext.todoItems.ToList();
        }
        public List<TodoItem> GetAllById(Guid id)
        {
            return _dbcontext.todoItems.Where(x => x.User.Id == id).ToList();
        }
        public void Toggle(Guid id)
        {
            var item = _dbcontext.todoItems.FirstOrDefault(x => x.Id == id);
            item.IsComplete = !item.IsComplete;
            _dbcontext.SaveChanges();
        }
        public void Add(TodoItem todoItem)
        {
            _dbcontext.todoItems.Add(todoItem);
            _dbcontext.SaveChanges();
        }
        public void Delete(Guid id)
        {
            var item = _dbcontext.todoItems.FirstOrDefault(x => x.Id == id);
            _dbcontext.Remove(item);
            _dbcontext.SaveChanges();
               
        }
        public void Update(Guid id, TodoItem todoItem)
        {
            var item = _dbcontext.todoItems.FirstOrDefault(x => x.Id == id);
            item.Title = todoItem.Title;
            item.Category = todoItem.Category;
            _dbcontext.todoItems.Update(item);
            _dbcontext.SaveChanges();
     
        }

    }
}
