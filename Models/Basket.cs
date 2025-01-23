using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

internal class Basket
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public List<Product>? Products { get; set; }
}
