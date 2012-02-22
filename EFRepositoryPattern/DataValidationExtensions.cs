using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace EFRepository
{
	public static class DataValidationExtensions
	{
		public static string CollectErrors(this DbEntityValidationException ex)
		{
			if (ex.EntityValidationErrors == null || ex.EntityValidationErrors.Count() == 0)
			{
				return String.Empty;
			}

			// Aggregate all the valiation errors into one string for display
			var errors = ex.EntityValidationErrors
				.SelectMany(validationErrors => validationErrors.ValidationErrors)
				.Aggregate(String.Empty, (current, validationError) =>
				                         current +
				                         String.Format("Property: {0} Error: {1}", validationError.PropertyName,
				                                       validationError.ErrorMessage));
			return errors;
		}
	}
}