using System;
using System.Collections.Generic;

namespace Goods.Models;

public partial class GoodsOrder
{
    public string GoodNumber { get; set; } = null!;

    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public virtual Good GoodNumberNavigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
