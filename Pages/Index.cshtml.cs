using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Models;
using TodoApp.Data;
using System.Linq;

namespace TodoApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TodoContext _context;

        public IndexModel(TodoContext context)
        {
            _context = context;
        }

        public IList<TodoItem> TodoList { get; set; } = new List<TodoItem>();

        [BindProperty]
        public TodoItem NewTodo { get; set; } = new TodoItem();

        public IActionResult OnPostAddTodo()
        {
            // Устанавливаем DateTimeKind как UTC
            NewTodo.StartTime = DateTime.SpecifyKind(NewTodo.StartTime, DateTimeKind.Utc);
            NewTodo.EndTime = DateTime.SpecifyKind(NewTodo.EndTime, DateTimeKind.Utc);

            // Проверяем ModelState
            if (!ModelState.IsValid)
                return Page();

            // Добавляем задачу в контекст
            _context.Todos.Add(NewTodo);

            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            return RedirectToPage();
        }


        public IActionResult OnPostToggleDone(int id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                todo.IsDone = !todo.IsDone;
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteTodo(int id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        public IList<TodoItem> SortedTodoList { get; set; } = new List<TodoItem>();

        public void OnGet()
        {
            SortedTodoList = _context.Todos
                .OrderBy(t => t.Date)
                .ThenBy(t => t.StartTime)
                .ToList();
        }
    }
}
