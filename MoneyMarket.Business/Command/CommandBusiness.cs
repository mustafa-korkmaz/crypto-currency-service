using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Business.Caching.Provider;
using MoneyMarket.DataAccess;

namespace MoneyMarket.Business.Command
{
    public class CommandBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.Command> _repository;

        public CommandBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.Command>();
        }

        /// <summary>
        /// cachable result for commands.
        /// max datetimeOffset cached
        /// </summary>
        /// <returns>command DTO object list</returns>
        [CacheableResult]
        public IEnumerable<Dto.Command> All()
        {
            var commands = _repository.AsQueryable()
                .Select(p => new Dto.Command
                {
                    Id = p.Id,
                    ScopeId = p.ScopeId,
                    Action = p.Action,
                    Text = p.Text,
                    Responses = p.Responses.Select(r => new Dto.Response
                    {
                        Id = r.Id,
                        Language = r.Language,
                        ErrorText = r.ErrorText,
                        SuccessText = r.SuccessText
                    }).DefaultIfEmpty()
                })
                .ToList();

            return commands;
        }
    }
}
