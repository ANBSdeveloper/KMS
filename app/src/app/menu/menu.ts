import { CoreMenu } from "@core/types";

export const menu: CoreMenu[] = [
  // Dashboard
  {
    id: "dashboard",
    title: "Dashboard",
    translate: "menu_dashboard",
    type: "collapsible",
    icon: "home",
    children: [
      {
        id: "analytics",
        title: "Analytics",
        translate: "menu_dashboard_analytics",
        type: "item",
        //permissions: ["Dashboards", "Dashboards.Admin"], // To set multiple role: ['Admin', 'Client']
        icon: "circle",
        url: "dashboard/analytics",
      },
    ],
  },

  {
    id: "invest",
    type: "section",
    title: "Investment",
    translate: "menu_investment",
    icon: "gear",
    children: [
      {
        id: "ticket-investment",
        title: "Ticket_Investment",
        translate: "menu_ticket_investment",
        permissions: [
          "TicketInvestments",
          "TicketInvestments.Register",
          "TicketInvestments.ApproveRequest",
          "TicketInvestments.DenyRequest",
          "TicketInvestments.ConfirmValid1",
          "TicketInvestments.DenyValid1",
          "TicketInvestments.ConfirmValid2",
          "TicketInvestments.DenyValid2",
          "TicketInvestments.ConfirmInvestment",
          "TicketInvestments.DenyInvestmentConfirmation",
          "TicketInvestments.ApproveInvestment1",
          "TicketInvestments.DenyInvestment1",
          "TicketInvestments.ApproveInvestment2",
          "TicketInvestments.DenyInvestment2",
          "TicketInvestments.UpdateProgress",
          "TicketInvestments.Operate",
          "TicketInvestments.Accept",
          "TicketInvestments.FinalSettlement",
          "TicketInvestments.SalesRemark",
          "TicketInvestments.CustomerDevelopmentRemark",
          "TicketInvestments.CompanyRemark",
        ],
        type: "item",
        icon: "folder",
        url: "investment/ticket-investment-list",
      },
      {
        id: "key-shop-approval",
        title: "KeyShopApprove",
        translate: "menu_key-shop-approval",
        permissions: ["Customers", "Customers.ApproveKeyShop"],
        type: "item",
        icon: "key",
        url: "master/key-shop-approval-list",
      },
      {
        id: "budget",
        title: "Budget",
        translate: "menu_budget",
        permissions: ["Budgets", "Budgets.AllocateArea", "Budgets.AllocateBranch"],
        type: "item",
        icon: "dollar-sign",
        url: "investment/budget-list",
      }, 
      {
        id: "order",
        title: "Order",
        translate: "menu_order",
        permissions: ["Orders"],
        type: "item",
        icon: "file-text",
        url: "investment/order-list",
      },
      {
        id: "ticket_print",
        title: "TicketPrint",
        translate: "menu_ticket_print",
        permissions: ["Tickets"],
        type: "item",
        icon: "printer",
        url: "investment/ticket-print",
      },
      {
        id: "ticketinvestmentcalendar",
        title: "ticketinvestmentcalendar",
        translate: "menu_ticket_investment_calendar",
        permissions: ["InvestmentSettings"],
        type: "item",
        icon: "list",
        url: "investment/ticket-investment-calendar",
      },
      {
        id: "product_point",
        title: "ProductPoint",
        translate: "menu_product_point",
        permissions: ["ProductPoints"],
        type: "item",
        icon: "credit-card",
        url: "master/product-point-list",
      },
      {
        id: "investment-setting",
        title: "InvestmentSetting",
        translate: "menu_investment_setting",
        permissions: ["InvestmentSettings"],
        type: "item",
        icon: "settings",
        url: "investment/investment-setting",
      },
      {
        id: "report",
        title: "Report",
        translate: "menu_report",
        type: "collapsible",
        icon: "pie-chart",
        children: [
          {
            id: "report_ticket_investment_result",
            title: "report_ticket_investment_result",
            translate: "report_ticket_investment_result",
            permissions: ["Reports", "Reports.TicketInvestments.Result"],
            type: "item",
            icon: "circle",
            url: "report/ticket-investment/result",
          },
          {
            id: "report_ticket_investment_ticket",
            title: "report_ticket_investment_ticket",
            translate: "report_ticket_investment_ticket",
            permissions: ["Reports", "Reports.TicketInvestments.Ticket"],
            type: "item",
            icon: "circle",
            url: "report/ticket-investment/ticket",
          },
          {
            id: "report_ticket_investment_remark",
            title: "report_ticket_investment_remark",
            translate: "report_ticket_investment_remark",
            permissions: ["Reports", "Reports.TicketInvestments.Remark"],
            type: "item",
            icon: "circle",
            url: "report/ticket-investment/remark",
          },
          {
            id: "report_ticket_investment_order_detail",
            title: "report_ticket_investment_order_detail",
            translate: "report_ticket_investment_order_detail",
            permissions: ["Reports", "Reports.TicketInvestments.Order"],
            type: "item",
            icon: "circle",
            url: "report/ticket-investment/order-detail",
          },
          {
            id: "report_ticket_investment_reward",
            title: "report_ticket_investment_reward",
            translate: "report_ticket_investment_reward",
            permissions: ["Reports", "Reports.TicketInvestments.Ticket"],
            type: "item",
            icon: "circle",
            url: "report/ticket-investment/reward",
          },
          {
            id: "report_ticket_investment_scan_qrcode",
            title: "report_ticket_investment_scan_qrcode",
            translate: "report_ticket_investment_scan_qrcode",
            permissions: ["Reports", "Reports.TicketInvestments.ScanQrCode"],
            type: "item",
            icon: "circle",
            url: "report/ticket-investment/scan-qrcode",
          },
        ],
      },
    ],
  },
  {
    id: "system",
    type: "section",
    title: "System",
    translate: "menu_system",
    icon: "gear",
    children: [
      {
        id: "notification",
        title: "notification",
        translate: "menu_notification",
        permissions: ["Notifications"],
        type: "item",
        icon: "message-square",
        url: "master/notification-list",
      },
      {
        id: "cycle",
        title: "cycle",
        translate: "menu_cycle",
        permissions: ["Cycles"],
        type: "item",
        icon: "clock",
        url: "master/cycle-list",
      },
      {
        id: "user",
        title: "User",
        translate: "menu_user",
        permissions: ["Users", "MasterData"],
        type: "item",
        icon: "user",
        url: "system/user-list",
      },
      {
        id: "role",
        title: "Role",
        translate: "menu_role",
        permissions: ["Roles"],
        type: "item",
        icon: "users",
        url: "system/role-list",
      },
      {
        id: "app-setting",
        title: "AppSetting",
        translate: "menu_app_setting",
        permissions: ["AppSettings"],
        type: "item",
        icon: "settings",
        url: "system/app-setting-list",
      },
    ],
  },
  {
    id: "apps",
    type: "section",
    title: "Master Data",
    translate: "menu_master_data",
    icon: "package",
    children: [
      {
        id: "customer",
        title: "customer",
        translate: "menu_customer",
        permissions: ["Customers"],
        type: "item",
        icon: "users",
        url: "master/customer-list",
      },
      {
        id: "reward_package",
        title: "RewardPackage",
        translate: "menu_reward_package",
        permissions: ["RewardPackages"],
        type: "item",
        icon: "gift",
        url: "master/reward-package-list",
      },
      {
        id: "product",
        title: "Product",
        translate: "menu_product",
        permissions: ["Products"],
        type: "item",
        icon: "box",
        url: "master/product-list",
      },
      {
        id: "branch",
        title: "Branch",
        translate: "menu_branch",
        permissions: ["Branches"],
        type: "item",
        icon: "home",
        url: "master/branch-list",
      },
      {
        id: "material",
        title: "Masterial",
        translate: "menu_material",
        permissions: ["Materials"],
        type: "item",
        icon: "box",
        url: "master/material-list",
      },

      {
        id: "brand",
        title: "Brand",
        translate: "menu_brand",
        permissions: ["Brands"],
        type: "item",
        icon: "folder",
        url: "master/brand-list",
      },
      {
        id: "product-class",
        title: "ProductClass",
        translate: "menu_product_class",
        permissions: ["ProductClasses"],
        type: "item",
        icon: "folder",
        url: "master/product-class-list",
      },
      {
        id: "sub-product-class",
        title: "SubProductClass",
        translate: "menu_sub_product_class",
        permissions: ["SubProductClasses"],
        type: "item",
        icon: "folder",
        url: "master/sub-product-class-list",
      },
      {
        id: "geography",
        title: "Geography",
        translate: "menu_geography",
        type: "collapsible",
        icon: "map",
        children: [
          {
            id: "zone",
            title: "Zone",
            translate: "menu_zone",
            permissions: ["Zones"],
            type: "item",
            icon: "circle",
            url: "master/zone-list",
          },
          {
            id: "area",
            title: "Area",
            translate: "menu_area",
            permissions: ["Areas"],
            type: "item",
            icon: "circle",
            url: "master/area-list",
          },
          {
            id: "province",
            title: "Province",
            translate: "menu_province",
            permissions: ["Provinces"],
            type: "item",
            icon: "circle",
            url: "master/province-list",
          },
          {
            id: "district",
            title: "District",
            translate: "menu_district",
            permissions: ["Districts"],
            type: "item",
            icon: "circle",
            url: "master/district-list",
          },
          {
            id: "ward",
            title: "Ward",
            translate: "menu_ward",
            permissions: ["Wards"],
            type: "item",
            icon: "circle",
            url: "master/ward-list",
          },
        ],
      },
    ],
  },
];