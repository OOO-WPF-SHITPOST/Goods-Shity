using System;
using System.Collections.Generic;

namespace Goods.Models;

public partial class Good
{
    public string Number { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int MessureUnitId { get; set; }

    public decimal Price { get; set; }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public int Discount { get; set; }

    public int QuantityInStock { get; set; }

    public string Description { get; set; } = null!;

    public string? Picture { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual MessureUnit MessureUnit { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
