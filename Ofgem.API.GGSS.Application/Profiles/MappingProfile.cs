using AutoMapper;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Commands.Users;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.Application.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            #region ENTITY-MODEL / MODEL-ENTITY MAPPING

            CreateMap<Entities.Address, AddressModel>().ReverseMap();
            CreateMap<Entities.Application, ApplicationModel>().ReverseMap();
            CreateMap<Entities.Document, DocumentModel>().ReverseMap();
            CreateMap<Entities.Organisation, OrganisationModel>().ReverseMap();
            CreateMap<Entities.ResponsiblePerson, ResponsiblePersonModel>().ReverseMap();
            CreateMap<Entities.ResponsiblePerson, ResponsiblePersonNomineeModel>().ReverseMap();
            CreateMap<Entities.User, UserModel>().ReverseMap();

            #endregion ENTITY-MODEL / MODEL-ENTITY MAPPING

            #region ENTITY-RESPONSE MAPPING

            CreateMap<Entities.Application, ApplicationResponse>();
            CreateMap<Entities.Organisation, OrganisationResponse>();

            #endregion ENTITY-RESPONSE MAPPING

            #region REQUEST-ENTITY MAPPING

            CreateMap<StageOne, ApplicationValue>();
            CreateMap<OrganisationSave, OrganisationValue>();
            CreateMap<OrganisationNominationSave, OrganisationValue>();

            CreateMap<Entities.User, UserRegistration>().ReverseMap();

            #endregion REQUEST-ENTITY MAPPING                  
        }
    }
}
