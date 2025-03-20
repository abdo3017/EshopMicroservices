using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string message): base(message) { }
        public NotFoundException(string entity, object key):base($"Entity \"{entity}\" ({key}) Was Not Found.")
        {
            
        }
    }
}
