using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using API_Proj.Infastructure;
using AutoMapper;
using API_Proj.Features;
using API_Proj.Features.DTO;
using API_Proj.Features.Request.Laptops;
using MediatR;
using API_Proj.Features.Controllers;
using Moq;
using AutoFixture;

namespace API_Proj.Test.Features.Controllers;
internal class LaptopsControllerTest {

    private LaptopsController _laptopsController;
    private readonly IMapper _mapper;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public LaptopsControllerTest(IMapper mapper) {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _mapper = mapper;
        _laptopsController = new LaptopsController(_mapper, _mediatorMock.Object);

    }
}


//    [TestClass]
//    public class UnitTest1 {
//        [TestMethod]
//        public void TestMethod1() {
//        }
//    }
//}