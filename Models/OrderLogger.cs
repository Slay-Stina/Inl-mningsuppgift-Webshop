using Assignment_Webshop.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlämningsuppgift_Webshop.Models;

internal class OrderLogger
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal TotalPrice { get; set; }
    public Order Order { get; set; }
}
