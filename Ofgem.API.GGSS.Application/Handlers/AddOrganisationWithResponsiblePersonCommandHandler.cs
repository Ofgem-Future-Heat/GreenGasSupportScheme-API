using AutoMapper;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class AddOrganisationWithResponsiblePersonCommandHandler : IRequestHandler<OrganisationSave, string>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;
        
        private readonly IMapper _mapper;

        public AddOrganisationWithResponsiblePersonCommandHandler(IMapper mapper, IOrganisationRepository organisationRepository, IResponsiblePersonRepository responsiblePersonRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organisationRepository = organisationRepository ?? throw new ArgumentNullException(nameof(organisationRepository));
            _responsiblePersonRepository = responsiblePersonRepository;
        }

        public async Task<string> Handle(OrganisationSave request, CancellationToken cancellationToken)
        {
            var organisation = _mapper.Map<Organisation>(request.Model);

            organisation.Value.LastModified = DateTime.Now.ToString("s");

            var responsiblePerson = organisation.ResponsiblePeople.First();
            responsiblePerson.UserId = Guid.Parse(request.Model.ResponsiblePeople.First().User.Id);
            responsiblePerson.User = null;

            organisation.ResponsiblePeople.Clear();

            var savedOrganisation =
                await _organisationRepository.AddAsync(organisation, new Guid(request.UserId), cancellationToken);


            await _responsiblePersonRepository.AddAsync(new ResponsiblePerson()
            {
                Id = Guid.NewGuid(),
                Documents = responsiblePerson.Documents,
                OrganisationId = savedOrganisation.Id,
                UserId = responsiblePerson.UserId,
                Value = responsiblePerson.Value
            }, Guid.Parse(request.UserId), cancellationToken);
            
            return savedOrganisation.Id.ToString();
        }
    }
}
