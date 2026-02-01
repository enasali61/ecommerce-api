using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ValidationErrorResponse
    {
        public int statusCode { get; set; }
        public string errorMessage { get; set; }
        public IEnumerable<ValidationError> Errors { get; set; }
    }
    public class ValidationError()
    {
        public string Feild { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
