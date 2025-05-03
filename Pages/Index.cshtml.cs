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

        public IList<TodoItem> SortedTodoList { get; set; } = new List<TodoItem>();

        public IActionResult OnPostAddTodo()
        {
            // Проверка на пустые поля перед тем, как обрабатывать
            if (!ModelState.IsValid)
                return Page();

            // Приведение времени в UTC, если оно не задано
            NewTodo.StartTime = DateTime.SpecifyKind(NewTodo.StartTime.ToUniversalTime(), DateTimeKind.Utc);
            NewTodo.EndTime = DateTime.SpecifyKind(NewTodo.EndTime.ToUniversalTime(), DateTimeKind.Utc);
            NewTodo.Date = DateTime.SpecifyKind(NewTodo.Date.Date, DateTimeKind.Utc);

            // Проверка на пересечение времени с другими задачами
            var overlappingTask = _context.Todos
                .Where(t => (t.StartTime < NewTodo.EndTime && t.EndTime > NewTodo.StartTime)) // Проверка на пересечение
                .FirstOrDefault();

            if (overlappingTask != null)
            {
                ModelState.AddModelError(string.Empty, "Уже существует задача в это время!");
                SortedTodoList = _context.Todos.OrderBy(t => t.Date).ToList(); // Обновляем список задач
                return Page();
            }

            if (NewTodo.StartTime == NewTodo.EndTime)
            {
                ModelState.AddModelError(string.Empty, "Время начала и окончания задачи не может совпадать.");
                SortedTodoList = _context.Todos.OrderBy(t => t.Date).ToList(); // Обновляем список задач
                return Page();
            }

            // Проверка: начало позже окончания
            if (NewTodo.StartTime > NewTodo.EndTime)
            {
                ModelState.AddModelError(string.Empty, "Время начала не может быть позже времени окончания.");
                SortedTodoList = _context.Todos.OrderBy(t => t.Date).ToList(); // Обновляем список задач
                return Page();
            }

            // Добавляем новую задачу
            _context.Todos.Add(NewTodo);
            _context.SaveChanges();

            // Обновляем список задач после добавления
            SortedTodoList = _context.Todos.OrderBy(t => t.Date).ThenBy(t => t.StartTime).ToList();

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

        public void OnGet()
        {
            // Загружаем список задач при загрузке страницы
            SortedTodoList = _context.Todos
                .OrderBy(t => t.Date)
                .ThenBy(t => t.StartTime)
                .ToList();
        }
    }
}