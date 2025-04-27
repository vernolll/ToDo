using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ќписание задачи об€зательно дл€ заполнени€.")]
        public string? Description { get; set; }
        public bool IsDone { get; set; }
    }
}
