using SohatNotebook.Entities.Dtos.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Entities.Dtos.Generic
{
    public class Result<T>
    {
        public T Content { get; set; }
        public Error Error { get; set; } = null;
        public bool IsSuccess { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}
