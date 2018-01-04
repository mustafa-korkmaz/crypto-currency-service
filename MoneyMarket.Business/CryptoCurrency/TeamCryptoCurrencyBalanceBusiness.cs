﻿using System.Linq;
using MoneyMarket.Common;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.CryptoCurrency
{
    public class TeamCryptoCurrencyBalanceBusiness : ICrudOperationBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.TeamCryptoCurrencyBalance> _repository;

        public string CurrentUserId { get; set; }

        public TeamCryptoCurrencyBalanceBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.TeamCryptoCurrencyBalance>();
        }

        #region CRUD operations

        public void Add(DtoBase dto)
        {
            var balanceDto = (Dto.TeamCryptoCurrencyBalance)dto;

            var entity = GetBalanceByNameAndCurrency(balanceDto);

            if (entity == null)
            {
                entity = MappingConfigurator.Mapper.Map<DataAccess.Models.TeamCryptoCurrencyBalance>(dto);

                _repository.Insert(entity);

                _uow.Save();
                dto.Id = entity.Id;

                return;
            }

            //data exists, update record.
            entity.Balance = balanceDto.Balance;

            _repository.Update(entity);

            _uow.Save();
        }

        public void Edit(DtoBase dto)
        {
            var teamDto = (Dto.Team)dto;

            var entity = _repository.GetById(teamDto.Id);

            entity.Name = teamDto.Name;
            _repository.Update(entity);

            _uow.Save();
        }

        /// <summary>
        /// delete for 0 balances
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _repository.Delete(id);
            _uow.Save();
        }

        public void Delete(DtoBase dto)
        {
            var balanceDto = (Dto.TeamCryptoCurrencyBalance)dto;

            var entity = GetBalanceByNameAndCurrency(balanceDto);

            if (entity != null)
            {
                _repository.Delete(entity);

                _uow.Save();
            }
        }

        public BusinessResponse<T> Get<T>(int id) where T : DtoBase
        {
            var businessResp = new BusinessResponse<T>
            {
                ResponseCode = ResponseCode.Fail
            };

            //var team = GetTeam(id);

            //if (team == null)
            //{
            //    businessResp.ResponseMessage = ErrorMessage.RecordNotFound;
            //    return businessResp;
            //}

            //businessResp.ResponseCode = ResponseCode.Success;
            //businessResp.ResponseData = team as T;

            return businessResp;
        }

        #endregion CRUD operations

        /// <summary>
        /// returns balance if exists by teamId, currency and name search filter
        /// </summary>
        /// <returns></returns>
        private DataAccess.Models.TeamCryptoCurrencyBalance GetBalanceByNameAndCurrency(Dto.TeamCryptoCurrencyBalance teamBalance)
        {
            return _repository.AsQueryable()
                .FirstOrDefault(p => p.Currency == teamBalance.Currency
                && p.Name == teamBalance.Name
                && p.TeamId == teamBalance.TeamId);
        }

    }
}