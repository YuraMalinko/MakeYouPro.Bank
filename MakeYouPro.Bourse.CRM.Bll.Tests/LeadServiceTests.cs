using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using Moq;
using NLog;
using FluentAssertions;
using MakeYouPro.Bourse.CRM.Bll.Mappings;
using NUnit.Framework;
using MakeYouPro.Bourse.CRM.Bll.Tests.TestCaseSource;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Bll.Tests
{
    public class LeadServiceTests
    {
        private LeadService _leadService;

        private Mock<ILeadRepository> _mockLeadRepo;

        private Mock<IAccountService> _mockAccountService;

        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperBllLeadProfile>();
                cfg.AddProfile<MapperBllAccountProfile>();
            }).CreateMapper();

            var logger = LogManager.Setup().GetCurrentClassLogger();

            _mockLeadRepo = new Mock<ILeadRepository>();
            _mockAccountService = new Mock<IAccountService>();
            _leadService = new LeadService(
                                            _mockLeadRepo.Object,
                                            _mockAccountService.Object,
                                            _mapper,
                                            logger);
        }

        [TestCaseSource(typeof(LeadServiceTestCaseSource), nameof(LeadServiceTestCaseSource.DeleteLeadByIdAsyncTestCaseSource))]
        public async Task DeleteLeadByIdAsyncTest(int leadId, LeadEntity leadEntity)
        {
            _mockLeadRepo.Setup(l => l.GetLeadByIdAsync(leadId)).ReturnsAsync(leadEntity);

            await _leadService.DeleteLeadByIdAsync(leadId);

            _mockLeadRepo.Verify(l => l.GetLeadByIdAsync(leadId), Times.Once);
            _mockLeadRepo.Verify(l => l.DeleteLeadByIdAsync(leadId), Times.Once);
            _mockAccountService.Verify(l => l.DeleteAccountByLeadIdAsync(leadId), Times.Once);
            _mockLeadRepo.VerifyNoOtherCalls();
            _mockAccountService.VerifyNoOtherCalls();
        }

        [TestCaseSource(typeof(LeadServiceTestCaseSource), nameof(LeadServiceTestCaseSource.DeleteLeadByIdAsyncTestCaseSource_WhenLeadStatusIsNotActive_ShouldBeArgumentException))]
        public void DeleteLeadByIdAsyncTest_WhenLeadStatusIsNotActive_ShouldBeArgumentException(int leadId, LeadEntity leadEntity)
        {
            _mockLeadRepo.Setup(l => l.GetLeadByIdAsync(leadId)).ReturnsAsync(leadEntity);

            Assert.ThrowsAsync<ArgumentException>(async () => await _leadService.DeleteLeadByIdAsync(leadId));

            _mockLeadRepo.Verify(l => l.GetLeadByIdAsync(leadId), Times.Once);
            _mockLeadRepo.Verify(l => l.DeleteLeadByIdAsync(leadId), Times.Never);
            _mockAccountService.Verify(l => l.DeleteAccountByLeadIdAsync(leadId), Times.Never);
            _mockLeadRepo.VerifyNoOtherCalls();
            _mockAccountService.VerifyNoOtherCalls();
        }

        [TestCaseSource(typeof(LeadServiceTestCaseSource), nameof(LeadServiceTestCaseSource.GetLeadByIdAsyncTestCaseSource))]
        public async Task GetLeadByIdAsyncTest(int leadId, LeadEntity leadEntity, Lead expected)
        {
            _mockLeadRepo.Setup(l => l.GetLeadByIdAsync(leadId)).ReturnsAsync(leadEntity);

           Lead actual =  await _leadService.GetLeadByIdAsync(leadId);

            _mockLeadRepo.Verify(l => l.GetLeadByIdAsync(leadId), Times.Once);
            _mockLeadRepo.VerifyNoOtherCalls();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(typeof(LeadServiceTestCaseSource), nameof(LeadServiceTestCaseSource.GetLeadByIdAsyncTestCaseSource_WhenTeadStatusIsNotActive_ShouldBeArgumentException))]
        public void GetLeadByIdAsyncTest_WhenTeadStatusIsNotActive_ShouldBeArgumentException(int leadId, LeadEntity leadEntity)
        {
            _mockLeadRepo.Setup(l => l.GetLeadByIdAsync(leadId)).ReturnsAsync(leadEntity);

           Assert.ThrowsAsync<ArgumentException>(async () => await _leadService.GetLeadByIdAsync(leadId));

            _mockLeadRepo.Verify(l => l.GetLeadByIdAsync(leadId), Times.Once);
            _mockLeadRepo.VerifyNoOtherCalls();
        }

        [TestCaseSource(typeof(LeadServiceTestCaseSource), nameof(LeadServiceTestCaseSource.CreateOrRecoverLeadAsyncTestCaseSource_WhenCreateLead))]
        public async Task CreateOrRecoverLeadAsyncTest_WhenCreateLead(LeadEntity leadEntity, List<LeadEntity> leads, LeadEntity addLeadEntity,
                                                                       Lead addLead, Lead expected)
        {
            _mockLeadRepo.Setup(l => l.GetLeadsByPassportEmailPhoneAsync(It.Is<LeadEntity>(l => Compare(l, leadEntity)))).ReturnsAsync(leads);
            _mockLeadRepo.Setup(l => l.CreateLeadAsync(It.Is<LeadEntity>(l => Compare(l , leadEntity)))).ReturnsAsync(addLeadEntity);

            Lead actual = await _leadService.CreateOrRecoverLeadAsync(addLead);

            _mockLeadRepo.Verify(l => l.GetLeadsByPassportEmailPhoneAsync(It.IsAny<LeadEntity>()), Times.Once);
            _mockLeadRepo.Verify(l => l.CreateLeadAsync(It.IsAny<LeadEntity>()), Times.Once);
            _mockAccountService.Verify(a => a.CreateOrRestoreAccountAsync(It.IsAny<Account>()), Times.Once);
            _mockLeadRepo.Verify(l => l.GetLeadByIdAsync(addLeadEntity.Id), Times.Never);
            _mockLeadRepo.Verify(l => l.UpdateLeadAsync(It.IsAny<LeadEntity>()), Times.Never);
            _mockLeadRepo.Verify(l => l.UpdateLeadPhoneNumberAsync("0", addLeadEntity.Id), Times.Never);
            _mockLeadRepo.Verify(l => l.RestoringDeletedStatusAsync(addLeadEntity.Id), Times.Never);
            _mockAccountService.VerifyNoOtherCalls();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(typeof(LeadServiceTestCaseSource), nameof(LeadServiceTestCaseSource.CreateOrRecoverLeadAsyncTestCaseSource_WhenRecoverLeadWithSamePasport))]
        public async Task CreateOrRecoverLeadAsyncTest_WhenRecoverLeadWithSamePasport(LeadEntity leadEntity, List<LeadEntity> leads, Lead leadMatchedDb,
                                                                                 LeadEntity leadEntityDb, LeadEntity leadUpdateEntity, Lead addLead, Lead expected)
        {
            _mockLeadRepo.Setup(l => l.GetLeadsByPassportEmailPhoneAsync(It.Is<LeadEntity>(l => Compare(l, leadEntity)))).ReturnsAsync(leads);
            _mockLeadRepo.Setup(l => l.GetLeadByIdAsync(leadMatchedDb.Id)).ReturnsAsync(leadEntityDb);
            _mockLeadRepo.Setup(l => l.UpdateLeadAsync(It.Is<LeadEntity>(l => Compare(l, leadEntityDb)))).ReturnsAsync(leadUpdateEntity);

            //  _mockLeadRepo.Setup(l => l.CreateLeadAsync(It.Is<LeadEntity>(l => Compare(l , leadEntity)))).ReturnsAsync(addLeadEntity);

            Lead actual = await _leadService.CreateOrRecoverLeadAsync(addLead);

            _mockLeadRepo.Verify(l => l.RestoringDeletedStatusAsync(leadMatchedDb.Id), Times.Once);
            _mockAccountService.Verify(a => a.CreateOrRestoreAccountAsync(It.IsAny<Account>()), Times.Once);
            _mockLeadRepo.Verify(l => l.GetLeadsByPassportEmailPhoneAsync(It.IsAny<LeadEntity>()), Times.Once);
            _mockLeadRepo.Verify(l => l.GetLeadByIdAsync(leadMatchedDb.Id), Times.Once);
            _mockLeadRepo.Verify(l => l.UpdateLeadAsync(It.IsAny<LeadEntity>()), Times.Once);
            _mockLeadRepo.Verify(l => l.CreateLeadAsync(It.IsAny<LeadEntity>()), Times.Never);
            _mockLeadRepo.Verify(l => l.UpdateLeadPhoneNumberAsync("0", leadMatchedDb.Id), Times.Never);
            _mockAccountService.VerifyNoOtherCalls();


            //_mockLeadRepo.Verify(l => l.GetLeadsByPassportEmailPhoneAsync(It.IsAny<LeadEntity>()), Times.Once);
            //_mockAccountService.Verify(a => a.CreateOrRestoreAccountAsync(It.IsAny<Account>()), Times.Once);
            //_mockLeadRepo.Verify(l => l.GetLeadByIdAsync(addLeadEntity.Id), Times.Never);
            //_mockLeadRepo.Verify(l => l.UpdateLeadAsync(It.IsAny<LeadEntity>()), Times.Never);
            //_mockLeadRepo.Verify(l => l.RestoringDeletedStatusAsync(addLeadEntity.Id), Times.Never);

            actual.Should().BeEquivalentTo(expected);
        }

        private bool Compare(LeadEntity l, LeadEntity leadEntity)
        {
            try
            {
                l.Should().BeEquivalentTo(leadEntity);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}