using Microsoft.AspNetCore.Identity;

namespace Todo.Models;

public class TodoUser : IdentityUser<Guid>
{
    public ICollection<TodoItem> TodoItems { get; set; }
}
