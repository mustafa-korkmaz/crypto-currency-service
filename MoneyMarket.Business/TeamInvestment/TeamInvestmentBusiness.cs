﻿using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Common;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.TeamInvestment
{
    public class TeamInvestmentBusiness : ICrudOperationBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.TeamInvestment> _repository;

        public string CurrentUserId { get; set; }

        public TeamInvestmentBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.TeamInvestment>();
        }

        #region CRUD operations

        public void Add(DtoBase dto)
        {
            var teamInvesmentDto = (Dto.TeamInvestment)dto;

            var entity = GetTeamTeamInvesmentByNameAndCurrency(teamInvesmentDto);

            if (entity == null)
            {
                entity = MappingConfigurator.Mapper.Map<DataAccess.Models.TeamInvestment>(dto);

                _repository.Insert(entity);

                _uow.Save();
                dto.Id = entity.Id;

                return;
            }

            //data exists, update record.
            entity.Balance = teamInvesmentDto.Balance;

            _repository.Update(entity);

            _uow.Save();
        }

        public void Edit(DtoBase dto)
        {
            var teamDto = (Dto.TeamInvestment)dto;

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
            var invesmentDto = (Dto.TeamInvestment)dto;

            var entity = GetTeamTeamInvesmentByNameAndCurrency(invesmentDto);

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

        public IEnumerable<Dto.TeamInvestment> GetTeamInvesments(int teamId, Currency currency)
        {
            var query = _repository.GetAsQueryable(p => p.TeamId == teamId);

            if (currency != Currency.Unknown)
            {
                query = query.Where(p => p.Currency == currency);
            }

            var invesments = query
                 .Select(p => new Dto.TeamInvestment
                 {
                     Id = p.Id,
                     Balance = p.Balance,
                     Currency = p.Currency,
                     Name = p.Name,
                     TeamId = teamId
                 })
                .ToList();

            return invesments;
        }

        /// <summary>
        /// returns teamInvesment if exists by teamId, currency and name search filter
        /// </summary>
        /// <returns></returns>
        private DataAccess.Models.TeamInvestment GetTeamTeamInvesmentByNameAndCurrency(Dto.TeamInvestment teamInvesment)
        {
            return _repository.AsQueryable()
                .FirstOrDefault(p => p.Currency == teamInvesment.Currency
                && p.Name == teamInvesment.Name
                && p.TeamId == teamInvesment.TeamId);
        }

    }
}