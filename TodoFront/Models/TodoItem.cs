using System.ComponentModel.DataAnnotations;

namespace TodoFront.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool isCompleted { get; set; }
}
