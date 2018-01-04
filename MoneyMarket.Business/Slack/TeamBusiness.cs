using System.Linq;
using MoneyMarket.Common;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.Slack
{
    public class TeamBusiness : ICrudOperationBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.Team> _repository;

        public string CurrentUserId { get; set; }

        public TeamBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.Team>();
        }

        #region CRUD operations

        public void Add(DtoBase dto)
        {
            var entity = MappingConfigurator.Mapper.Map<DataAccess.Models.Team>(dto);

            _repository.Insert(entity);

            _uow.Save();

            dto.Id = entity.Id;
        }

        public void Edit(DtoBase dto)
        {
            var teamDto = (Dto.Team)dto;

            var entity = _repository.GetById(teamDto.Id);

            entity.AccountType = teamDto.AccountType;
            entity.BotAccessToken = teamDto.BotAccessToken;
            entity.Name = teamDto.Name;
            entity.BotId = teamDto.BotId;
            entity.ExpiresIn = teamDto.ExpiresIn;
            entity.Language = teamDto.Language;
            entity.IsActive = teamDto.IsActive;
            entity.SlackId = teamDto.SlackId;
            entity.MemberCount = teamDto.MemberCount;
            entity.MainCurrency = teamDto.MainCurrency;
            entity.Provider = teamDto.Provider;

            _repository.Update(entity);

            _uow.Save();
        }

        public void Delete(int id)
        {
            SoftDelete(id);
        }

        public BusinessResponse<T> Get<T>(int id) where T : DtoBase
        {
            var businessResp = new BusinessResponse<T>
            {
                ResponseCode = ResponseCode.Fail
            };

            var team = GetTeam(id);

            if (team == null)
            {
                businessResp.ResponseMessage = ErrorMessage.RecordNotFound;
                return businessResp;
            }

            businessResp.ResponseCode = ResponseCode.Success;
            businessResp.ResponseData = team as T;

            return businessResp;
        }

        #endregion CRUD operations

        /// <summary>
        /// returns team if exists by slackTeamId search filter
        /// </summary>
        /// <param name="slackTeamId"></param>
        /// <returns></returns>
        public Dto.Team GetTeamBySlackId(string slackTeamId)
        {
            var teamScopeRepository = _uow.Repository<DataAccess.Models.TeamScope>();

            var query = from t in _repository.AsQueryable()
                        join ts in teamScopeRepository.AsQueryable()
                        on t.Id equals ts.TeamId
                        into leftJoin
                        from p in leftJoin.DefaultIfEmpty()
                        where t.SlackId == slackTeamId
                        select new Dto.TeamScope
                        {
                            Team = new Team
                            {
                                Id = t.Id,
                                AccountType = t.AccountType,
                                BotAccessToken = t.BotAccessToken,
                                Name = t.Name,
                                BotId = t.BotId,
                                ExpiresIn = t.ExpiresIn,
                                Language = t.Language,
                                IsActive = t.IsActive,
                                SlackId = t.SlackId,
                                MainCurrency = t.MainCurrency,
                                MemberCount = t.MemberCount,
                                Provider = t.Provider,
                                CreatedAt = t.CreatedAt
                            },
                            Id = p.Id,
                            ScopeId = p.ScopeId
                        };

            var teamScopes = query.ToList();

            var teamDto = teamScopes.Select(p => p.Team).FirstOrDefault();

            if (teamDto != null)
            {
                teamDto.TeamScopes = teamScopes;

                return teamDto;
            }

            return null;
        }

        //public IEnumerable<Dto.WebSite> All()
        //{
        //    var webSites = _repository.AsQueryable()
        //        .Select(p => new Dto.WebSite
        //        {
        //            Id = p.Id,
        //            Domain = p.Domain,
        //            ImageUrl = p.ImageUrl,
        //            Name = p.Name,
        //            TrackedProductTotal = p.TrackedProductTotal,
        //        })
        //        .OrderByDescending(ws => ws.TrackedProductTotal)
        //        .ToList();

        //    return webSites;
        //}

        private Dto.Team GetTeam(int teamId)
        {
            var teamDto = _repository.GetAsQueryable(s => s.Id == teamId)
                .Select(p => new Dto.Team
                {
                    Id = p.Id,
                    AccountType = p.AccountType,
                    BotAccessToken = p.BotAccessToken,
                    Name = p.Name,
                    BotId = p.BotId,
                    ExpiresIn = p.ExpiresIn,
                    Language = p.Language,
                    IsActive = p.IsActive,
                    SlackId = p.SlackId,
                    MainCurrency = p.MainCurrency,
                    MemberCount = p.MemberCount,
                    Provider = p.Provider,
                    CreatedAt = p.CreatedAt
                    //todo: also fetch scopes 
                }).FirstOrDefault();

            return teamDto;
        }

        /// <summary>
        /// soft deletion for teams.
        /// </summary>
        /// <param name="id"></param>
        private void SoftDelete(int id)
        {
            var entity = _repository.GetById(id);

            entity.IsActive = false;

            _repository.Update(entity);

            _uow.Save();
        }


    }
}