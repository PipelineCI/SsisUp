using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsisUp.Builders.References
{
    /// <summary>
    /// Specifies the duration in which to execute the job throughout the day. 
    /// </summary>
    public enum FrequencySubdayType
    {
        AtSpecifiedTime = 1,
        Minute = 4,
        Hour = 8
    }
}
