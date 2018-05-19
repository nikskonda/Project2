using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.exception
{
    public class SQLWorkerException : Exception
    {

        public SQLWorkerException(string message)
            : base(message)
        { }

    }
}
