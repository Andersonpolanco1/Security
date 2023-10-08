using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CustomAuth
{
    public class CustomSchemeOptions
    {
        public bool AddBearerAuth { get; set; } = false;
        public bool AddAPiKeyAuth { get; set; } = false;
    }
}
