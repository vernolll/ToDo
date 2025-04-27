using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "�������� ������ ����������� ��� ����������.")]
        public string? Description { get; set; }
        public bool IsDone { get; set; }
    }
}
