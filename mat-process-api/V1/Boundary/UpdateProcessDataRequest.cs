using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mat_process_api.V1.Domain;

namespace mat_process_api.V1.Boundary
{
    public class UpdateProcessDataRequest
    {
        public MatUpdateProcessData processDataToUpdate { get; set; }
    }
}
