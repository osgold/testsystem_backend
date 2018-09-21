﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testsystem.Interfaces.Repositories;
using testsystem.Interfaces.Services;
using testsystem.Models.Dto;
using testsystem.Models.Entities;

namespace testsystem.Services
{
    public class CandidatService : ICandidatService
    {

        private readonly IPositionRepository _positionRepository;
        private readonly ICandidatRepositories _candidatRepositories;
        private readonly ITestRepository _testRepository;
        private readonly IAnswerService _answerService;
      //  private readonly ITestService _testService;


        public CandidatService(IPositionRepository positionRepository, ICandidatRepositories candidatRepositories, IAnswerService answerService,
        ITestRepository testRepository)
        {
            this._positionRepository = positionRepository;
            this._candidatRepositories = candidatRepositories;
        //    this._testRepository = testRepository;
        //    _testService = testService;
           _answerService = answerService;
        }

        public bool AddCandidat(CandidatDto dto)
        {
            var model = GetModel(dto);
            var candidatId = this._candidatRepositories.AddCandidat(model);

            var answer = true;

          //   var tests = GetTests(model.Position.Id);
           // var tests = _testService.GetTests(model.PositionId);


            answer = this._answerService.AddAnswerByCandidat(candidatId, model.PositionId);

            return answer;

        }

        public CandidatDto GetCandidat(int id)
        {

            var model = _candidatRepositories.GetCandidat(id);
            if (model == null)
            {
                return null;
            }

            return GetDto(model);
        }

        public bool DeleteCandidat(int id)
        {
            return this._candidatRepositories.RemoveCandidat(id);
        }

        public IEnumerable<CandidatDto> GetCandidats(int id)
        {
            var models = this._candidatRepositories.GetCandidats(id);
            var dtos = new List<CandidatDto>();

            foreach(var model in models)
            {
                var dto = this.GetDto(model);
                dtos.Add(dto);
            }
            return dtos;
        }



        public bool UpdateCandidat(CandidatDto dto)
        {
            throw new NotImplementedException();
        }


        private Candidat GetModel(CandidatDto candidatDto)
        {
            var candidatModel = new Candidat
            {
                Name = candidatDto.Name,
                Email = candidatDto.Email,
                ExpiredDate = candidatDto.ExpiredDate,
                InvitationDate = candidatDto.InvitationDate,
                Number = candidatDto.Number,
                Phone = candidatDto.Phone,
            };
            candidatModel.Position = this._positionRepository.GetPositionWithoutIncludes(candidatDto.PositionId);

            return candidatModel;
        }

        private CandidatDto GetDto (Candidat candidatModel)
        {
            var candidatDto = new CandidatDto()
            {
                Id = candidatModel.Id,
                Name = candidatModel.Name,
                Email = candidatModel.Email,
                ExpiredDate = candidatModel.ExpiredDate,
                InvitationDate = candidatModel.InvitationDate,
                Number = candidatModel.Number,
                PositionId = candidatModel.PositionId
            };
            return candidatDto;
        }

    }
}
