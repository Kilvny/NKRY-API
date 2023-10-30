using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.ResourceParameters
{
    public class InvoicesResourceParameters : ResourceParameters
    {
        public string? Date { get; set; }
        public string? DateFrom { get; set; }
        public string? InvoiceNumber { get; set; }

    }
}