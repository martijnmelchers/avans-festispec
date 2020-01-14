using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Attributes
{
    public sealed class ListElements : ValidationAttribute
    {
        private readonly int _maxElements;
        private readonly int _minElements;

        public ListElements(int minElements, int maxElements = -1)
        {
            _minElements = minElements;
            _maxElements = maxElements;
        }

        public override bool IsValid(object value)
        {
            if (value is ICollection list)
                return list.Count >= _minElements && (_maxElements <= 0 || list.Count <= _maxElements);
            return false;
        }
    }
}