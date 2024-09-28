using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Entities.Dtos.Generic
{
    public class ResultPage<T> : Result<T> where T : class
    {
        public List<T> Content { get; set; }
        public int ResultPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
