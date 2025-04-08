using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Common.OutputMessages
{
    public static class ErrorMessages
    {
        public const string EntityImportError = 
            "There were errors while importing {0} entity ! The following errors were observed: ";
        
        public const string ReferencedEntityMissing = 
            "One or more of the referenced entities by the dto is not present in the DB";
        
        public const string EntityInstanceAlreadyExists = 
            "One or more of the referenced entities is skipped due to existing record with the same data";
    }
}
