using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using API_Proj.Infastructure;
using AutoMapper;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;

namespace API_Proj.Test.Features.Controllers;
internal class OfficesControllerTest {

    private OfficesController _officesController;
    private readonly IMapper _mapper;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public OfficesControllerTest(IMapper mapper) {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _mapper = mapper;
        _officesController = new OfficesController(_mapper, _mediatorMock.Object);

    }
}
