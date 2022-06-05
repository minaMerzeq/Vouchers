using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Controllers;
using Vouchers.Data.Repos;
using Vouchers.Dtos;
using Vouchers.Models;
using Vouchers.Profiles;
using Xunit;

namespace Vouchers.Tests.Controllers
{
    public class MerchantsControllerTests
    {
        private readonly Mock<IMerchantRepo> _repoMock;
        private readonly IMapper _mapperMock;
        private readonly MerchantsController _sut; // system under test

        public MerchantsControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MerchantsProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            _repoMock = new Mock<IMerchantRepo>();
            _mapperMock = mapper;
            
            _sut = new MerchantsController(_repoMock.Object, _mapperMock); //creates the implementation in-memory
        }

        [Fact]
        public void GetAllMerchants_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var merchantsMock = new List<Merchant>();
            _repoMock.Setup(x => x.GetAllMerchants()).Returns(merchantsMock);

            //Act
            var result = _sut.GetAllMerchants();

            //Assert
            //Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(List<MerchantReadDto>));
            _repoMock.Verify(x => x.GetAllMerchants(), Times.Once());
        }
    }
}
