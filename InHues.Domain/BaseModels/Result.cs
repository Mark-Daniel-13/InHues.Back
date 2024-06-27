using System.Collections.Generic;
using System.Linq;

namespace InHues.Domain.BaseModels
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors,object data)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Value = data;
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public object Value { get; set; }

        public static Result Success(object respond = null)
        {
            return new Result(true, new string[] { }, respond);
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors, null);
        }
    }
}
