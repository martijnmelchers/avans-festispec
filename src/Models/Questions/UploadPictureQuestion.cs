using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using System.Collections.Generic;

namespace Festispec.Models.Questions
{
    public class UploadPictureQuestion : Question, IAnswerable<FileAnswer>
    {
        public UploadPictureQuestion(string contents, Questionnaire questionnaire) : base(contents, questionnaire) { }
        public UploadPictureQuestion() : base() { }

        public override GraphType GraphType => GraphType.None;
        public new virtual ICollection<FileAnswer> Answers { get; set; }
    }
}