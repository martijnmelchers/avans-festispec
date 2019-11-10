using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models
{
    public interface IAnswer<out TAnswer>
    {
        TAnswer GetAnswer();
    }
}