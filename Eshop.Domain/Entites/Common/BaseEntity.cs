using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Entites.Common;

public class BaseEntity
{
    [Key]
    public long Id { get; set; }
    public string? UserName { get; set; }
    public bool IsDelete { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}