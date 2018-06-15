using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenService.Models
{
    public class ResultsPdf
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Results { get; set; }
    }
}
