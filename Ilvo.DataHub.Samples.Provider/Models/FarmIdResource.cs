using System;
using System.Collections.Generic;

namespace Ilvo.DataHub.Samples.Provider.Models
{
    public class FarmIdResource
    {
        public string Resource { get; set; }
        public IEnumerable<string> FarmIds { get; set; }
    }
}
