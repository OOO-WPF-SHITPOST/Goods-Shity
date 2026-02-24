using System;
using System.Collections.Generic;
using System.Text;

namespace Goods.Windows
{
    public class FormValidation
    {
        public bool Validate(string goodName, string price, string discount, string quantityInStock)
        {
            if (string.IsNullOrWhiteSpace(goodName))
                return false;

            if (goodName.Length < 2 || goodName.Length > 200)
                return false;

            if (!decimal.TryParse(price, out var parsedPrice))
                return false;

            if (parsedPrice <= 0)
                return false;

            if (parsedPrice > 1_000_000_000)
                return false;

            if (!int.TryParse(discount, out var parsedDiscount))
                return false;

            if (parsedDiscount < 0 || parsedDiscount > 100)
                return false;

            if (!int.TryParse(quantityInStock, out var parsedQuantity))
                return false;

            if (parsedQuantity < 0)
                return false;

            return true;
        }
    }
}
