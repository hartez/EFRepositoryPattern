using System;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;

namespace EFRepository
{
	public class DataValidationException : Exception
	{
		public DataValidationException()
		{
		}

		public DataValidationException(string message)
			: base(message)
		{
		}

		private readonly string _relevantInnnerExceptionMessage;

		public DataValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
			if (innerException != null && innerException is DbUpdateException)
			{
				// We can drill down to get a more useful message
				var moreRelevantException = FindRelevantException(innerException);

				if (moreRelevantException != null)
				{
					_relevantInnnerExceptionMessage = moreRelevantException.Message;
				}
			}
		}

		public override string Message
		{
			get
			{
				if (!String.IsNullOrEmpty(_relevantInnnerExceptionMessage))
				{
					return string.Format("{0}: {1}", base.Message, _relevantInnnerExceptionMessage);
				}

				return base.Message;
			}
		}

		private static Exception FindRelevantException(Exception ex)
		{
			// TODO Obviously this has some internationalization issues; need to find another way of determining that 
			// this is the exception that we want
			if (ex.Message.ToLower().Contains("see the inner exception for details") && ex.InnerException != null)
			{
				return FindRelevantException(ex.InnerException);
			}

			return ex;
		}

		protected DataValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}