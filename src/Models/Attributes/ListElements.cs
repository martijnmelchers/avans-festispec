using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Attributes
{
    public sealed class ListElements : ValidationAttribute
    {
        private readonly int _minElements;
        private readonly int _maxElements;
        public ListElements(int minElements, int maxElements = -1)
        {
            _minElements = minElements;
            _maxElements = maxElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as ICollection;
            if (list != null)
                return list.Count >= _minElements && (_maxElements > 0 ? list.Count <= _maxElements : true);
            return false;
        }
    }
}
