using System.Collections.Generic;

namespace Features
{
    public class OperationResult
    {
        public OperationResult(bool succeeded)
        {
            Succeeded = succeeded;
            ErrorMessages = new List<string>();
        }
        public bool Succeeded { get; protected set; }
        public IEnumerable<string> ErrorMessages { get; protected set; }
    }
}
