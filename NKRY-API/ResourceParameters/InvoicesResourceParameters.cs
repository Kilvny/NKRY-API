using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.ResourceParameters
{
    public class InvoicesResourceParameters : ResourceParameters
    {
        public DateOnly Date { get; set; }
        public DateOnly DateFrom { get; set; }

    }
}