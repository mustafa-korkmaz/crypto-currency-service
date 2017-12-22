using MoneyMarket.Common.Response;
using MoneyMarket.Dto;

namespace MoneyMarket.Business
{
    interface ICrudOperationBusiness
    {
        void Add(DtoBase dto);

        void Edit(DtoBase dto);

        void Delete(int id);

        BusinessResponse<T> Get<T>(int id) where T : DtoBase;

        /// <summary>
        /// user who executes the crud operation. (IDOR control)
        /// </summary>
        string CurrentUserId { get; set; }
    }
}
