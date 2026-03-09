using Moq;
using Xunit;
using System;

namespace lab31vn15
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public interface ICampaignRepository
    {
        Campaign GetCampaign(int id);
        void SaveCampaign(Campaign campaign);
    }

    public interface IAnalyticsService
    {
        void TrackCampaignCreated(string name);
        void TrackCampaignActivated(int id);
    }

    public class CampaignService
    {
        private readonly ICampaignRepository repository;
        private readonly IAnalyticsService analytics;

        public CampaignService(ICampaignRepository repository, IAnalyticsService analytics)
        {
            this.repository = repository;
            this.analytics = analytics;
        }

        public Campaign CreateCampaign(string name)
        {
            var campaign = new Campaign
            {
                Name = name,
                IsActive = false
            };

            repository.SaveCampaign(campaign);
            analytics.TrackCampaignCreated(name);

            return campaign;
        }

        public void ActivateCampaign(int id)
        {
            var campaign = repository.GetCampaign(id);

            if (campaign == null)
                throw new Exception();

            campaign.IsActive = true;

            repository.SaveCampaign(campaign);
            analytics.TrackCampaignActivated(id);
        }
    }

    public class CampaignServiceTests
    {
        [Fact]
        public void Test1()
        {
            var repo = new Mock<ICampaignRepository>();
            var analytics = new Mock<IAnalyticsService>();
            var service = new CampaignService(repo.Object, analytics.Object);

            service.CreateCampaign("Test");

            repo.Verify(r => r.SaveCampaign(It.IsAny<Campaign>()), Times.Once);
        }

        [Fact]
        public void Test2()
        {
            var repo = new Mock<ICampaignRepository>();
            var analytics = new Mock<IAnalyticsService>();
            var service = new CampaignService(repo.Object, analytics.Object);

            service.CreateCampaign("Test");

            analytics.Verify(a => a.TrackCampaignCreated("Test"), Times.Once);
        }

        [Fact]
        public void Test3()
        {
            var repo = new Mock<ICampaignRepository>();
            var analytics = new Mock<IAnalyticsService>();
            var service = new CampaignService(repo.Object, analytics.Object);

            var result = service.CreateCampaign("New");

            Assert.Equal("New", result.Name);
        }

        [Fact]
        public void Test4()
        {
            var campaign = new Campaign { Id = 1, IsActive = false };

            var repo = new Mock<ICampaignRepository>();
            repo.Setup(r => r.GetCampaign(1)).Returns(campaign);

            var analytics = new Mock<IAnalyticsService>();

            var service = new CampaignService(repo.Object, analytics.Object);

            service.ActivateCampaign(1);

            Assert.True(campaign.IsActive);
        }

        [Fact]
        public void Test5()
        {
            var campaign = new Campaign { Id = 1 };

            var repo = new Mock<ICampaignRepository>();
            repo.Setup(r => r.GetCampaign(1)).Returns(campaign);

            var analytics = new Mock<IAnalyticsService>();

            var service = new CampaignService(repo.Object, analytics.Object);

            service.ActivateCampaign(1);

            repo.Verify(r => r.SaveCampaign(campaign), Times.Once);
        }

        [Fact]
        public void Test6()
        {
            var campaign = new Campaign { Id = 1 };

            var repo = new Mock<ICampaignRepository>();
            repo.Setup(r => r.GetCampaign(1)).Returns(campaign);

            var analytics = new Mock<IAnalyticsService>();

            var service = new CampaignService(repo.Object, analytics.Object);

            service.ActivateCampaign(1);

            analytics.Verify(a => a.TrackCampaignActivated(1), Times.Once);
        }

        [Fact]
        public void Test7()
        {
            var repo = new Mock<ICampaignRepository>();
            repo.Setup(r => r.GetCampaign(1)).Returns((Campaign)null);

            var analytics = new Mock<IAnalyticsService>();

            var service = new CampaignService(repo.Object, analytics.Object);

            Assert.Throws<Exception>(() => service.ActivateCampaign(1));
        }

        [Fact]
        public void Test8()
        {
            var campaign = new Campaign { Id = 1 };

            var repo = new Mock<ICampaignRepository>();
            repo.Setup(r => r.GetCampaign(1)).Returns(campaign);

            var analytics = new Mock<IAnalyticsService>();

            var service = new CampaignService(repo.Object, analytics.Object);

            service.ActivateCampaign(1);

            repo.Verify(r => r.GetCampaign(1), Times.Once);
        }
    }
}