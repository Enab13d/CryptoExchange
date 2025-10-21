using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContracts
{
    public class FiatToCryptoResponseMessage
    {
        public Guid CorrelationId { get; set; }
        public string Status { get; set; } = default!;
    }
}
