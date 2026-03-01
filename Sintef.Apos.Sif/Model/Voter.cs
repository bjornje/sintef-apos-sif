using System.Collections.ObjectModel;

namespace Sintef.Apos.Sif.Model
{
    public class Voter
    {
        private readonly Node _parent;
        private readonly Attribute<int?> _numberOfElements;
        private readonly Attribute<int?> _threshhold;
        private readonly string _threshholdName;
        private readonly string _numberOfElementsName;

        public Voter(Node parent, Attribute<int?> threshhold, string threshholdName, Attribute<int?> numberOfElements, string numberOfElementsName) 
        {
            _parent = parent;
            _threshhold = threshhold;
            _threshholdName = threshholdName;
            _numberOfElements = numberOfElements;
            _numberOfElementsName = numberOfElementsName;
        }

        public void Validate(Collection<ModelError> errors)
        {
            long? M = null;
            long? N = null;
            var actualNumberOfElements = _parent.GetNumberOfElements();

            if (string.IsNullOrEmpty(_threshhold.StringValue))
            {
                errors.Add(new ModelError(_parent, $"Value for voting parameter {_threshholdName} is missing."));
            }

            if (long.TryParse(_threshhold.StringValue, out var m))
            {
                if (m < 1) errors.Add(new ModelError(_parent, $"Voting parameter {_threshholdName} must be >= 1."));
                M = m;
            }

            if (string.IsNullOrEmpty(_numberOfElements.StringValue))
            {
                errors.Add(new ModelError(_parent, $"Value for total number of elements {_numberOfElementsName} is missing."));
            }

            if (long.TryParse(_numberOfElements.StringValue, out var n))
            {
                if (n < 1) errors.Add(new ModelError(_parent, $"The total number of elements {_numberOfElementsName} in a voting must be >= 1."));
                if (n != actualNumberOfElements)
                {
                    errors.Add(new ModelError(_parent, $"The actual number of elements = {actualNumberOfElements} differes from the intended number {_numberOfElementsName} = {n}."));

                }
                N = n;
            }

            if (M.HasValue && N.HasValue && M.Value > N.Value)
            {
                errors.Add(new ModelError(_parent, $"In voting {_threshholdName}oo{_numberOfElementsName} we must have {_threshholdName} <= {_numberOfElementsName}. Here {_threshholdName} is {M} and {_numberOfElementsName} is {N}."));
            }
        }
    }
}
