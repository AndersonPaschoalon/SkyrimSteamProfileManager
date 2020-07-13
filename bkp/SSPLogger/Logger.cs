using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger_
{
    public interface Logger
    {

        void Debug();

        void Info();

        void Warn();

        void Error();

    }
}
