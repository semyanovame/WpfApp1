using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models;

public partial class Производители
{
    public int ИдПроизводителя { get; set; }

    public string? Наименование { get; set; }

    public virtual ICollection<Товары> Товары { get; set; } = new List<Товары>();
}