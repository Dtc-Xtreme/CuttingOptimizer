using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Exceptions
{
    public class CreateSvgLoopException : Exception
    {
        public CreateSvgLoopException(string message):base(message)
        {
        }
    }
}
