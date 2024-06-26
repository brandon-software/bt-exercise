﻿using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using Microsoft.AspNetCore.Http;

namespace StargateAPI.Business.Commands
{
    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreatePersonPreProcessor> _logger;

        public CreatePersonPreProcessor(StargateContext context, ILogger<CreatePersonPreProcessor> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(z => z.Name == request.Name);

            if (person is not null) throw new BadHttpRequestException("Bad Request");

            _logger.LogInformation($"CreatePerson Request OK: Name - {request.Name}");
        }
    }

    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreatePersonHandler> _logger;

        public CreatePersonHandler(StargateContext context, ILogger<CreatePersonHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {
            var newPerson = new Person
            {
                Name = request.Name
            };

            await _context.People.AddAsync(newPerson, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);


            _logger.LogInformation($"Person with ID: {newPerson.Id} and Name: {newPerson.Name} created");

            return new CreatePersonResult()
            {
                Id = newPerson.Id
            };
        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}