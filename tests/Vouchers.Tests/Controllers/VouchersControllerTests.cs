using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using AutoFixture;
using Vouchers.Data.Repos;
using Vouchers.Controllers;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Vouchers.Models;
using Microsoft.AspNetCore.Mvc;
using Vouchers.Dtos;
using Vouchers.Profiles;

namespace Vouchers.Tests.Controllers
{
    public class VouchersControllerTests
    {
        private readonly Mock<IVoucherRepo> _repoMock;
        private readonly IMapper _mapperMock;
        private readonly IConfiguration _configMock;
        private readonly Mock<IPurchaseRepo> _purchaseRepoMock;
        private readonly VouchersController _sut; // system under test

        public VouchersControllerTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"SpentPoints:Silver", "70"},
                {"SpentPoints:Gold", "100"},
                {"SpentPoints:Platinum", "150"},
            };

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new VouchersProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            _repoMock = new Mock<IVoucherRepo>();
            _mapperMock = mapper;
            _configMock = new ConfigurationBuilder()
                            .AddInMemoryCollection(myConfiguration)
                            .Build();
            _purchaseRepoMock = new Mock<IPurchaseRepo>();
            _sut = new VouchersController(_repoMock.Object, _mapperMock, _configMock, _purchaseRepoMock.Object); //creates the implementation in-memory
        }

        [Fact]
        public void GetAllVouchers_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var vouchersMock = new List<Voucher>();
            _repoMock.Setup(x => x.GetAllVouchers()).Returns(vouchersMock);

            //Act
            var result = _sut.GetAllVouchers();

            //Assert
            //Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(List<VoucherReadDto>));
            _repoMock.Verify(x => x.GetAllVouchers(), Times.Once());
        }

        [Fact]
        public void GetVoucherById_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var voucherMock = new Voucher();
            var id = 1;
            _repoMock.Setup(x => x.GetVoucherById(id)).Returns(voucherMock);

            //Act
            var result = _sut.GetVoucherById(id);

            //Assert
            //Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(VoucherReadDto));
            _repoMock.Verify(x => x.GetVoucherById(id), Times.Once());
        }

        [Fact]
        public void GetVoucherById_ShouldReturnNotFound_WhenNoDataFound()
        {
            //Arrange
            Voucher voucherMock = null;
            var id = 1;
            _repoMock.Setup(x => x.GetVoucherById(id)).Returns(voucherMock);

            //Act
            var result = _sut.GetVoucherById(id);

            //Assert
            //Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _repoMock.Verify(x => x.GetVoucherById(id), Times.Once());
        }

        [Fact]
        public void CreateVoucher_ShouldReturnOkResponse_WhenValidRequest()
        {
            //Arrange
            var request = new VoucherCreateDto() { Title="test", Description="test", CostPoints=20, AvailableNumberOfTimes = 2, Criteria=0, Type=0, Image="", MerchantId = 2 };
            var voucher = new Voucher() { Title = "test", Description = "test", CostPoints = 20, AvailableNumberOfTimes = 2, Criteria = 0, Type = 0, Image = "", MerchantId = 2 };
            _repoMock.Setup(x => x.CreateVoucher(voucher));
            _repoMock.Setup(x => x.GetVoucherById(voucher.Id)).Returns(voucher);

            //Act
            var result = _sut.CreateVoucher(request);

            //Assert
            //Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<CreatedAtRouteResult>();
            result.Result.As<CreatedAtRouteResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(VoucherReadDto));

            _repoMock.Verify(x => x.CreateVoucher(voucher), Times.Once());
            _repoMock.Verify(x => x.GetVoucherById(voucher.Id), Times.Once());
        }

        [Fact]
        public void CreateVoucher_ShouldReturnBadRequest_WhenInvalidRequest()
        {
            //Arrange
            var request = new VoucherCreateDto();
            var voucher = new Voucher();
            _sut.ModelState.AddModelError("Type", "Type should be an int"); // make request invalid
            _repoMock.Setup(x => x.CreateVoucher(voucher));

            //Act
            var result = _sut.CreateVoucher(request);

            //Assert
            //Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestResult>();

            _repoMock.Verify(x => x.CreateVoucher(voucher), Times.Never());
        }
    }
}
