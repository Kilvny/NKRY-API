using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.ResourceParameters
{
    public class InvoicesResourceParameters : ResourceParameters
    {
        public DateTime Date { get; set; }
        public DateTime DateFrom { get; set; }

    }
}