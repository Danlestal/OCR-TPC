using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatcherCmd.Jobs.Interfaces
{
    public interface IOCRJobFactory
    {
        IList<OCRJob> GetOCRJobs();
    }
}
