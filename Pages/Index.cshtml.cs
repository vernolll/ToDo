using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Models;
using System.Collections.Generic;

namespace TodoApp.Pages
{
    public class IndexModel : PageModel
    {
        public static readonly List<TodoItem> TodoList = new List<TodoItem>(); //  ВНИМАНИЕ:  Используется статический список для хранения данных в памяти. Для постоянного хранения используйте базу данных.

        [BindProperty]
        public TodoItem NewTodo { get; set; } = new TodoItem();

        public void OnGet()
        {
        }

        public IActionResult OnPostAddTodo()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewTodo.Id = TodoList.Count > 0 ? TodoList.Max(t => t.Id) + 1 : 1;
            TodoList.Add(NewTodo);
            NewTodo = new TodoItem(); // Clear the NewTodo property

            return RedirectToPage();
        }


        public IActionResult OnPostToggleDone(int id)
        {
            var todo = TodoList.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                todo.IsDone = !todo.IsDone;
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteTodo(int id)
        {
            var todo = TodoList.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                TodoList.Remove(todo);
            }
            return RedirectToPage();
        }
    }
}
