using System;
using System.Collections.Generic;

namespace Goods.Models;

public partial class GoodsDeliveryPoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
