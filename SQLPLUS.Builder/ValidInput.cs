using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLPLUS.Builder
{
    /// <summary>
    /// Services as the base class for all input models.
    /// </summary>
    public abstract class ValidInput
    {
        /// <summary>
        /// List of ValidationResults populated during IsValid() call.
        /// </summary>
        public List<ValidationResult> ValidationResults { private set; get; } = new List<ValidationResult>();

        /// <summary>
        /// Validates the object according to the annotations assigned by the SQL Plus tags.
        /// When the method returns false the ValidationErrors will have a count > 1.
        /// </summary>
        /// <returns>True|False based on the valid state of the object.</returns>
        public virtual bool IsValid()
        {
            ClearErrors();
            Validator.TryValidateObject(this, new ValidationContext(this), ValidationResults, true);
            return ValidationResults.Count == 0;
        }

        /// <summary>
        /// Clears ValidationResults of any previous errors.
        /// </summary>
        public virtual void ClearErrors()
        {
            ValidationResults.Clear();
        }
    }
}
