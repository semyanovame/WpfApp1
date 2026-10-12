using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models;

public partial class Товары
{
    public string? Артикул { get; set; }

    public string? Наименование { get; set; }

    public string? ЕдиницаИзмерения { get; set; }

    public double? Цена { get; set; }

    public int ВкПоставщики { get; set; }

    public int ВкПроизводители { get; set; }

    public int ВкКатегории { get; set; }

    public double? Скидка { get; set; }

    public double? НаСкладе { get; set; }

    public string? Описание { get; set; }

    public string? Фото { get; set; }

    public double ИдТовары { get; set; }

    public virtual Категории ВкКатегорииNavigation { get; set; } = null!;

    public virtual Поставщики ВкПоставщикиNavigation { get; set; } = null!;

    public virtual Производители ВкПроизводителиNavigation { get; set; } = null!;
}