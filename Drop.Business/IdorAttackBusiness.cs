
namespace MoneyMarket.Business
{
    public abstract class IdorAttackBusiness
    {
        /// <summary>
        /// real user id get from token
        /// </summary>
        public string CurrentUserId { get; set; }

        /// <summary>
        /// verifies if requestUser id is the same as in token
        /// </summary>
        /// <param name="requestUserId"></param>
        /// <returns></returns>
        public bool VerifyUser(string requestUserId)
        {
            return CurrentUserId == requestUserId;
        }
    }
}
