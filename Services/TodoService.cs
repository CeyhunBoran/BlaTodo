using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
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
            return _dbcontext.todoItems.ToList();
        }
        public List<TodoItem> GetAllById(Guid id)
        {
            return _dbcontext.todoItems.Where(x => x.User.Id == id).ToList();
        }
        public async Task<List<TodoItem>> GetTodoItemsCached(Guid id)
        {
            List<TodoItem> todoItems;
            todoItems = await Task.Run(() => _memoryCache.Get<List<TodoItem>>("todos"));
            if (!_memoryCache.TryGetValue("todos", out todoItems))
            {
                todoItems = await Task.Run(() => _dbcontext.todoItems.Where(x => x.User.Id == id).ToList());
                _memoryCache.Set("todos", todoItems, TimeSpan.FromSeconds(10));
                await Task.Delay(3000); //For simulating lots of query
            }
            return todoItems;
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
