using System.Collections.Generic;

namespace Ilvo.DataHub.Samples.Provider.Models
{
    public class FarmIdRequest
    {
        public string ResourceUrl { get; set; }
        public IEnumerable<string> FarmIds { get; set; }
        public bool All { get; set; }
    }
}
