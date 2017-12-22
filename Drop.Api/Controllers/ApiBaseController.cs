using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using MoneyMarket.Api.Filters;
using MoneyMarket.Common;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// Base controller class for all controllers
    /// </summary>
    [MoneyMarketHeaderAuthorization]  // needs api key  and channelType
    [ExceptionHandler]
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// return model state errors as a sentence.
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        protected string GetModelStateErrors(ModelStateDictionary dic)
        {
            var list = dic.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            string result = string.Empty;

            for (int i = 1; i <= list.Count(); i++)
            {
                result += list[i - 1];

                if (i < list.Count())
                {
                    result += " ";
                }
            }

            return result.Trim();
        }

        /// <summary>
        /// return validation result errors as a sentence.
        /// </summary>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        protected string GetModelValidationResultErrors(IEnumerable<ValidationResult> validationResults)
        {
            return validationResults.First().ErrorMessage;
        }

        /// <summary>
        /// returns channel type as webAdmin, Ios or android
        /// </summary>
        /// <returns></returns>
        protected string GetChannelType()
        {
            IEnumerable<string> headerValues;

            Request.Headers.TryGetValues(RequestHeader.ChannelType, out headerValues);

            return headerValues.FirstOrDefault();
        }
    }
}
