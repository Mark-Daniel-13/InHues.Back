using System.Collections.Generic;

namespace InHues.Domain.BaseModels
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public List<string> Errors { get; set; }
    }
}
