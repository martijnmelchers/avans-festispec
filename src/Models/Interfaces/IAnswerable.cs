using System.Collections.Generic;
using Festispec.Models.Answers;

namespace Festispec.Models.Interfaces
{
    public interface IAnswerable<TAnswer> where TAnswer : Answer
    {
        ICollection<TAnswer> Answers { get; set; }
    }
}