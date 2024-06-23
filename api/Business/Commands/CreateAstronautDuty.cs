using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using Microsoft.Extensions.Logging;
using System.Net;

namespace StargateAPI.Business.Commands
{
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        public required string Name { get; set; }

        public required string Rank { get; set; }

        public required string DutyTitle { get; set; }

        public DateTime DutyStartDate { get; set; }
    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreateAstronautDutyPreProcessor> _logger;

        public CreateAstronautDutyPreProcessor(StargateContext context, ILogger<CreateAstronautDutyPreProcessor> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(z => z.Name == request.Name);

            if (person is null) throw new BadHttpRequestException("Bad Request");

            var verifyNoPreviousDuty = await _context.AstronautDuties.FirstOrDefaultAsync(z => z.DutyTitle == request.DutyTitle && z.DutyStartDate == request.DutyStartDate);

            if (verifyNoPreviousDuty is not null) throw new BadHttpRequestException("Bad Request");

            _logger.LogInformation($"Astronaut Duty Request OK: Name - {request.Name}, Rank - {request.Rank}, DutyTitle - {request.DutyTitle}, DutyStartDate - {request.DutyStartDate}");
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreateAstronautDutyHandler> _logger;
        public CreateAstronautDutyHandler(StargateContext context, ILogger<CreateAstronautDutyHandler> logger)
        {
            _context = context;
            _logger = logger;

        }
        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var query = "SELECT * FROM [Person] WHERE Name = @Name";
            var person = await _context.Connection.QueryFirstOrDefaultAsync<Person>(query, new { Name = request.Name });

            if (person is null) throw new BadHttpRequestException("Bad Request");

            query = "SELECT * FROM [AstronautDetail] WHERE PersonId = @PersonId";
            var astronautDetail = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDetail>(query, new { PersonId = person.Id });

            if (astronautDetail == null)
            {
                astronautDetail = new AstronautDetail
                {
                    PersonId = person.Id,
                    CurrentDutyTitle = request.DutyTitle,
                    CurrentRank = request.Rank,
                    CareerStartDate = request.DutyStartDate.Date
                };

                if (request.DutyTitle == "RETIRED")
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.Date;
                }

                await _context.AstronautDetails.AddAsync(astronautDetail);
                _logger.LogInformation($"New Astronaut Detail created with ID: {astronautDetail.Id}");
            }
            else
            {
                astronautDetail.CurrentDutyTitle = request.DutyTitle;
                astronautDetail.CurrentRank = request.Rank;
                if (request.DutyTitle == "RETIRED")
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }
                _context.AstronautDetails.Update(astronautDetail);
                _logger.LogInformation($"Astronaut Detail with ID: {astronautDetail.Id} updated");
            }

            query = "SELECT * FROM [AstronautDuty] WHERE PersonId = @PersonId AND DutyEndDate IS NULL ORDER BY DutyStartDate DESC";
            var astronautDuty = await _context.Connection.QueryFirstOrDefaultAsync<AstronautDuty>(query, new { PersonId = person.Id });

            if (astronautDuty != null)
            {
                astronautDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
                _context.AstronautDuties.Update(astronautDuty);
                _logger.LogInformation($"Astronaut Duty with ID: {astronautDuty.Id} DutyEndDate updated to {astronautDuty.DutyEndDate}");
            }

            var newAstronautDuty = new AstronautDuty()
            {
                PersonId = person.Id,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate.Date,
                DutyEndDate = null
            };

            await _context.AstronautDuties.AddAsync(newAstronautDuty);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"New Astronaut Duty created with ID: {newAstronautDuty.Id}");
            return new CreateAstronautDutyResult
            {
                Id = newAstronautDuty.Id
            };
        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}