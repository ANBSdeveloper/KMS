using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Localization.Sources;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.Orders.Actions
{
    public class OrderCreateAction : IEntityAction
    {
        public OrderCreateAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId,
            string consumerPhone,
            string consumerName,
            bool checkSpoon,
            string api,
            List<OrderDetail> orderDetails,
            Action<OrderCreateResult> getResult)
        {
            ConsumerPhone = consumerPhone;
            ConsumerName = consumerName;
            UserId = userId;
            OrderDetails = orderDetails;
            CheckSpoon = checkSpoon;
            Api = api;
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            GetResult = getResult;
        }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public string ConsumerPhone { get; private set; }
        public string ConsumerName { get; private set; }
        public int UserId { get; private set; }
        public bool CheckSpoon { get; private set; }
        public string Api { get; private set; }
        public List<OrderDetail> OrderDetails { get; private set; }
        public Action<OrderCreateResult> GetResult { get; set; }
        public class OrderCreateResult
        {
            public List<Ticket> ActualNewTickets { get; set; }
            public bool OutOfTicket { get; set; }
        }
        public class OrderDetail
        {
            public string CompareProductCode { get; private set; }
            public string SpoonCode { get; private set; }
            public string QrCode { get; private set; }

            public OrderDetail(string compareProductCode, string qrCode, string spoonCode)
            {
                CompareProductCode = compareProductCode;
                SpoonCode = spoonCode;
                QrCode = qrCode;
            }
        }
    }
}