using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    internal partial class EmptyStatus : Status
    {
        public static EmptyStatus Instance { get; } = new EmptyStatus("");

        public EmptyStatus(object value) : base(value) { } 
    }
}
