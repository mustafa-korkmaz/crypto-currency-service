using PostSharp.Aspects;
using PostSharp.Serialization;

namespace MoneyMarket.Business.Exception
{
    /// <summary>
    /// Runs when method get exception.
    /// use this attribute when your business not wrapped with ExceptionHandler attribute or includes async sub methods without try-catch blocks.
    /// </summary>
    [PSerializable]
    class AppException : OnExceptionAspect
    {
        /// <summary>
        /// Runs when method get exception.
        /// </summary>
        /// <param name="args"></param>
        public override void OnException(MethodExecutionArgs args)
        {
            var excBusiness = new ExceptionBusiness();
            excBusiness.SaveExceptionAsync(args.Exception);
        }
    }
}
