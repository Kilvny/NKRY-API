using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.ResourceParameters
{
    public class AdminsResourceParameters : ResourceParameters
    {
        public string? DateFrom { get; set; }
        public string? AdminId { get; set; }
    }
}