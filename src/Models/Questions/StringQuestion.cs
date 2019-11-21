using System.Collections.Generic;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;

namespace Festispec.Models.Questions
{
    public class StringQuestion : Question, IAnswerable<StringAnswer>
    {
        public const int CharacterLimit = 400;

        public bool IsMultiline { get; set; }

        public override GraphType GraphType => GraphType.None;

        public new virtual ICollection<StringAnswer> Answers { get; set; }
    }
}