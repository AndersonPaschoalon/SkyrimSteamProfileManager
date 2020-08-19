using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {

        void Debug(string msg);

        void Info(string msg);

        void Warn(string msg);

        void Error(string msg);


    }
}
