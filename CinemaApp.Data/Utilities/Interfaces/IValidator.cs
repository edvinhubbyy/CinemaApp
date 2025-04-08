using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Utilities.Interfaces
{
    public interface IValidator
    {
        IReadOnlyCollection<string> ErrorMessages { get; }
        bool IsValid(object obj);

    }
}
