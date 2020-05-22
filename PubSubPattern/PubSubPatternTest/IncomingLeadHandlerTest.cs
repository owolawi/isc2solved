using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubSubPattern;
using System.Collections.Generic;

namespace PubSubPatternTest
{
    [TestClass]
    public class IncomingLeadHandlerTest
    {
        public static List<Lead> GetLeads()
        {
            return ProcessedLeadsStorage.ProcessedLeads;
        }

        [TestInitialize]
        public void RemoveAllProcessLeads()
        {
            ProcessedLeadsStorage.ProcessedLeads.Clear();
        }

        [TestMethod]
        public void HandleMessage_NullChannel_ExpectThrowsArgumentNullException()
        {
            //Arrange
            IncomingLeadHandler testHandler = new IncomingLeadHandler();

            //Act and Assert
            Assert.ThrowsException<System.ArgumentNullException>(() => testHandler.HandleMessage(null, null));
        }

        [TestMethod]
        public void HandleMessage_ValidLead_ExpectInsert()
        {
            //Arrange
            Lead testLead = new Lead()
            {
                FirstName = "Test",
                LastName = "Tester",
                Company = "ISC2",
                Email = "example@example.com"
            };

            IncomingLeadHandler testHandler = new IncomingLeadHandler();

            //Act
            PubSubService.Instance.Subscribe(testHandler);

            int leadsBeforePublishCount = GetLeads().Count;
            PubSubService.Instance.Publish(IncomingLeadHandler.INCOMING_LEAD_CHANNEL, testLead);
            List<Lead> leadsAfterPublish = GetLeads();

            //Assert
            Assert.AreEqual(0, leadsBeforePublishCount, "Expect no leads.");
            Assert.AreEqual(1, leadsAfterPublish.Count, "Expect one lead.");
            Assert.AreEqual(testLead.FirstName, leadsAfterPublish[0].FirstName, "Expect the test lead.");
        }

        [TestMethod]
        public void HandleMessage_InvalidLead_ExpectThrowsArgumentException()
        {
            //Arrange
            Lead testLead = new Lead()
            {
                FirstName = null,
                LastName = "Tester",
                Company = "ISC2",
                Email = "example@example.com"
            };

            IncomingLeadHandler testHandler = new IncomingLeadHandler();

            //Act
            PubSubService.Instance.Subscribe(testHandler);

            List<Lead> leads = GetLeads();

            //Act and Assert
            Assert.ThrowsException<System.ArgumentException>(() => PubSubService.Instance.Publish(IncomingLeadHandler.INCOMING_LEAD_CHANNEL, testLead));
            Assert.AreEqual(0, leads.Count, "Expect no leads.");
        }

        [TestMethod]
        public void HandleMessage_InvalidChannel_WithLead_ExpectNoLeadInserted()
        {
            //Arrange
            Lead testLead = new Lead()
            {
                FirstName = "Test",
                LastName = "Tester",
                Company = "ISC2",
                Email = "example@example.com"
            };

            IncomingLeadHandler testHandler = new IncomingLeadHandler();

            //Act
            PubSubService.Instance.Subscribe(testHandler);

            int leadsBeforePublishCount = GetLeads().Count;
            PubSubService.Instance.Publish("nope", testLead);
            List<Lead> leadsAfterPublish = GetLeads();

            //Assert
            Assert.AreEqual(0, leadsBeforePublishCount, "Expect no leads.");
            Assert.AreEqual(0, leadsAfterPublish.Count, "Expect no leads.");
        }

        [TestMethod]
        public void HandleMessage_NotSubscribed_ExpectNoLeadInserted()
        {
            //Arrange
            Lead testLead = new Lead()
            {
                FirstName = "Test",
                LastName = "Tester",
                Company = "ISC2",
                Email = "example@example.com"
            };

            IncomingLeadHandler testHandler = new IncomingLeadHandler();

            //Act
            PubSubService.Instance.Subscribe(testHandler);
            PubSubService.Instance.Unsubscribe(testHandler);

            int leadsBeforePublishCount = GetLeads().Count;
            PubSubService.Instance.Publish(IncomingLeadHandler.INCOMING_LEAD_CHANNEL, testLead);
            List<Lead> leadsAfterPublish = GetLeads();

            //Assert
            Assert.AreEqual(0, leadsBeforePublishCount, "Expect no leads.");
            Assert.AreEqual(0, leadsAfterPublish.Count, "Expect no leads.");
        }

        [TestMethod]
        public void HandleMessage_MultipleSameSubscribers_ExpectOneLeadInserted()
        {
            //Arrange
            Lead testLead = new Lead()
            {
                FirstName = "Test",
                LastName = "Tester",
                Company = "ISC2",
                Email = "example@example.com"
            };

            IncomingLeadHandler testHandler = new IncomingLeadHandler();

            //Act
            PubSubService.Instance.Subscribe(testHandler);
            PubSubService.Instance.Subscribe(testHandler);
            PubSubService.Instance.Unsubscribe(testHandler);
            PubSubService.Instance.Subscribe(testHandler);

            int leadsBeforePublishCount = GetLeads().Count;
            PubSubService.Instance.Publish(IncomingLeadHandler.INCOMING_LEAD_CHANNEL, testLead);
            List<Lead> leadsAfterPublish = GetLeads();

            //Assert
            Assert.AreEqual(0, leadsBeforePublishCount, "Expect no leads.");
            Assert.AreEqual(1, leadsAfterPublish.Count, "Expect one leads.");
        }

        [TestMethod]
        public void HandleMessage_MultipleDifferentSubscribers_ExpectOneLeadInserted()
        {
            //Arrange
            Lead testLead = new Lead()
            {
                FirstName = "Test",
                LastName = "Tester",
                Company = "ISC2",
                Email = "example@example.com"
            };

            IncomingLeadHandler testHandler = new IncomingLeadHandler();
            IncomingLeadHandler testHandlerLms = new IncomingLeadHandler("LMS");
            IncomingLeadHandler testHandlerOkta = new IncomingLeadHandler("OKTA");

            //Act
            PubSubService.Instance.Subscribe(testHandler);
            PubSubService.Instance.Subscribe(testHandlerLms);
            PubSubService.Instance.Subscribe(testHandlerOkta);

            int leadsBeforePublishCount = GetLeads().Count;
            PubSubService.Instance.Publish(IncomingLeadHandler.INCOMING_LEAD_CHANNEL, testLead);
            List<Lead> leadsAfterPublish = GetLeads();

            //Assert
            Assert.AreEqual(0, leadsBeforePublishCount, "Expect no leads.");
            Assert.AreEqual(3, leadsAfterPublish.Count, "Expect three leads.");
        }

        [TestMethod]
        public void HandleMessage_MultipleDifferentSubscribersWithOneUsubscribe_ExpectOneLeadInserted()
        {
            //Arrange
            Lead testLead = new Lead()
            {
                FirstName = "Test",
                LastName = "Tester",
                Company = "ISC2",
                Email = "example@example.com"
            };

            IncomingLeadHandler testHandler = new IncomingLeadHandler();
            IncomingLeadHandler testHandlerLms = new IncomingLeadHandler("LMS");
            IncomingLeadHandler testHandlerOkta = new IncomingLeadHandler("OKTA");

            //Act
            PubSubService.Instance.Subscribe(testHandler);
            PubSubService.Instance.Subscribe(testHandlerLms);
            PubSubService.Instance.Subscribe(testHandlerOkta);
            PubSubService.Instance.Unsubscribe(testHandlerOkta);

            int leadsBeforePublishCount = GetLeads().Count;
            PubSubService.Instance.Publish(IncomingLeadHandler.INCOMING_LEAD_CHANNEL, testLead);
            List<Lead> leadsAfterPublish = GetLeads();

            //Assert
            Assert.AreEqual(0, leadsBeforePublishCount, "Expect no leads.");
            Assert.AreEqual(2, leadsAfterPublish.Count, "Expect two leads.");
        }
    }
}
