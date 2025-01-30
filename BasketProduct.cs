using Assignment_Webshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop;

internal class BasketProduct
{
    public int Id { get; set; }
    public int BasketId { get; set; }
    public virtual Basket Basket { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Quantity { get; set; }
}


