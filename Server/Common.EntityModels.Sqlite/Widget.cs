using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.EntityModels.Sqlite;

public class Widget
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public string? WidgetName { get; set; }
}
