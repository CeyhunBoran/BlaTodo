using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using Todo.Data;
using Todo.Models;

namespace Todo.Services
{
    public class TodoService
    {
        protected readonly TodoDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;

        public TodoService(TodoDbContext dbcontext, IMemoryCache memoryCache)
        {
            _dbcontext = dbcontext;
            this._memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public List<TodoItem> GetAll()
        {
            return GetTodoItemsCached(t => true);
        }
        public List<TodoItem> GetAllById(Guid id)
        {
            return GetTodoItemsCached(t =>t.UserId == id);
        }
        public List<TodoItem> GetTodoItemsCached(Func<TodoItem, bool> query)
        {
            List<TodoItem> todoItems;
            todoItems = _memoryCache.Get<List<TodoItem>>("todos");
            if (todoItems != null)
                return todoItems.Where(query).ToList();
            
            todoItems = _dbcontext.todoItems.Where(query).ToList();
            _memoryCache.Set("todos", todoItems);
            return todoItems;
        }
        public void Toggle(Guid id)
        {
            var item = _dbcontext.todoItems.FirstOrDefault(x => x.Id == id);
            item.IsComplete = !item.IsComplete;
            _dbcontext.SaveChanges();
            var todolist = _memoryCache.Get<List<TodoItem>>("todos");
            if (todolist != null)
            {
                var index = todolist.FindIndex(t => t.Id == id);
                todolist[index] = item;
                _memoryCache.Set<List<TodoItem>>("todos", todolist);
            }
        }
        public void Add(TodoItem todoItem)
        {
            _dbcontext.todoItems.Add(todoItem);
            _dbcontext.SaveChanges();
            var todolist = _memoryCache.Get<List<TodoItem>>("todos");
            if (todolist != null)
            {
                todolist.Add(todoItem);
                _memoryCache.Set<List<TodoItem>>("todos", todolist);
            }
        }
        public void Delete(Guid id)
        {
            var item = _dbcontext.todoItems.FirstOrDefault(x => x.Id == id);
            _dbcontext.Remove(item);
            _dbcontext.SaveChanges();
            var todolist = _memoryCache.Get<List<TodoItem>>("todos");
            if (todolist != null)
            {
                todolist = todolist.Where(t => t.Id != id).ToList();
                _memoryCache.Set<List<TodoItem>>("todos", todolist);
            }
        }
        public void Update(Guid id, TodoItem todoItem)
        {
            var item = _dbcontext.todoItems.FirstOrDefault(x => x.Id == id);
            item.Title = todoItem.Title;
            item.Category = todoItem.Category;
            _dbcontext.todoItems.Update(item);
            _dbcontext.SaveChanges();
            var todolist = _memoryCache.Get<List<TodoItem>>("todos");
            if (todolist != null)
            {
                var index =todolist.FindIndex(t => t.Id == id);
                todolist[index] = item;
                _memoryCache.Set<List<TodoItem>>("todos", todolist);
            }
        }

    }
}
