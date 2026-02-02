using System;
using System.Collections.Generic;

namespace Goods.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int GoodsDeliveryPointId { get; set; }

    public int ClientId { get; set; }

    public int ReceiptCode { get; set; }

    public int StatusId { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual GoodsDeliveryPoint GoodsDeliveryPoint { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
