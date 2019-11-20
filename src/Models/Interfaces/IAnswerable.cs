using Festispec.Models.Answers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Interfaces
{
    public interface IAnswerable<TAnswer> where TAnswer : Answer
    {
        ICollection<TAnswer> Answers { get; set; }
    }
}
