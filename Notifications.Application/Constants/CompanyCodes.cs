using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Constants
{
    public class CompanyCodes
    {
        public static Dictionary<string, string> GetCompanies()
        {
            return new Dictionary<string, string>
            {
                { "DSR", "2001" },
                { "DCP", "1000" },
                { "NASCON", "2000" }
            };
        }
    }
}
