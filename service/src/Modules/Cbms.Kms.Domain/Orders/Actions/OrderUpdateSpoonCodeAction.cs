using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.Orders.Actions
{
    public class OrderUpdateSpoonCodeAction : IEntityAction
    {
        public class OrderUpdateSpoonResult
        {
            public int Result { get; set; }
            public string Message { get; set; }
            public decimal? Points { get; set; }
            public decimal? RequiredPoints { get; set; }
            public DateTime? OperationDate { get; set; }
            public DateTime? IssueTicketEndDate { get; set; }
            public string ShopCode { get; set; }
            public string ShopName { get; set; }
            public string ShopAddress { get; set; }
            public List<OrderUpdateSpoonResultTicket> Tickets { get; set; }
        }

        public class OrderUpdateSpoonResultTicket
        {
            public string Code { get; set; }
            public bool IsNew { get; set; }
        }

        public OrderUpdateSpoonCodeAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            string phone,
            string name,
            string qrCode,
            string spoonCode,
            Action<OrderUpdateSpoonResult> getResult
            )
        {
            Phone = phone;
            Name = name;
            QrCode = qrCode;
            SpoonCode = spoonCode;
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            GetResult = getResult;
        }

        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public string Phone { get; private set; }
        public string Name { get; private set; }
        public string QrCode { get; private set; }
        public string SpoonCode { get; private set; }
        public Action<OrderUpdateSpoonResult> GetResult { get; set; }
    }
}