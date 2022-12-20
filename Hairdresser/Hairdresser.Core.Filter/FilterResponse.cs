using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Filter
{
    public class FilterResponse<T>
    {
        public Filter FilterUsed { get; set; }
        public int TotalCount { get; set; }
        public int ResultsFound { get; set; }
        public List<T> List { get; set; }
    }
}
