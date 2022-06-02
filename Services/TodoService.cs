using Todo.Data;
namespace Todo.Services
{
    public class TodoService
    {
        protected readonly TodoDbContext _dbcontext;

        public TodoService(TodoDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
    }
}
