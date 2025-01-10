using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public virtual List<Product> Products { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public float UnitPrice { get; set; }
}
