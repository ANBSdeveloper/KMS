import {
  Component,
  Injector,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from "@angular/core";

import { CoreConfigService } from "@core/services/config.service";

import { AuthUser } from "app/auth/models";
import { colors } from "app/colors.const";
import { AuthenticationService } from "app/auth/service";
import { DashboardService } from "app/main/dashboard/dashboard.service";

import { AppComponentBase } from "@cbms/ng-core-vuexy";
import { ReportService } from "@app/main/report/report.service";
import moment from "moment";
import { formatNumber } from "devextreme/localization";

@Component({
  selector: "app-dashboard-analytics",
  templateUrl: "./analytics.component.html",
  styleUrls: ["./analytics.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class AnalyticsComponent extends AppComponentBase implements OnInit {
  // Decorator
  @ViewChild("orderBarChartRef") orderBarChartRef: any;
  @ViewChild("investmentLineChartRef") investmentLineChartRef: any;
  @ViewChild("budgetTotalChartRef") budgetTotalChartRef: any;
  @ViewChild("revenueReportChartRef") revenueReportChartRef: any;
  @ViewChild("budgetChartRef") budgetChartRef: any;
  @ViewChild("statePrimaryChartRef") statePrimaryChartRef: any;
  @ViewChild("stateWarningChartRef") stateWarningChartRef: any;
  @ViewChild("stateSecondaryChartRef") stateSecondaryChartRef: any;
  @ViewChild("stateInfoChartRef") stateInfoChartRef: any;
  @ViewChild("stateDangerChartRef") stateDangerChartRef: any;
  @ViewChild("goalChartRef") goalChartRef: any;

  // Public
  public data: any;
  public currentUser: AuthUser;
  public isAdmin: boolean;
  public isClient: boolean;
  public orderBar;
  public rewardBar;
  public investmentLine;
  public actualLine;
  public revenueReportChartoptions;
  public budgetChartoptions;
  public goalChartoptions;
  public statePrimaryChartoptions;
  public stateWarningChartoptions;
  public stateSecondaryChartoptions;
  public stateInfoChartoptions;
  public stateDangerChartoptions;
  public budgetTotalChartoptions;

  // Private
  private $barColor = "#f3f3f3";
  private $trackBgColor = "#EBEBEB";
  private $textMutedColor = "#b9b9c3";
  private $budgetStrokeColor2 = "#dcdae3";
  private $goalStrokeColor2 = "#51e5a8";
  private $textHeadingColor = "#5e5873";
  private $strokeColor = "#ebe9f1";

  cycles = [];
  currentCycle = undefined;
  budget: {
    AllocateAmount: number;
    UsedAmount: number;
    RemainAmount: number;
  };
  investment12MonthsData: {
    YearMonth: string;
    ActualAmount: number;
    InvestmentAmount;
  }[] = [];

  analytic6MonthsData: {
    YearMonth: string;
    ActualAmount: number;
    InvestmentAmount;
    Orders: number;
    RewardQuantity: number;
  }[] = [];

  analytic: {
    Shops: number;
    KeyShops: number;
    TicketShops: number;
    InvestAmount: number;
    RewardAmount: number;
    MaterialAmount: number;
    AvgInvestmentAmount: number;
    RewardQuantity: number;
    Orders: number;
    ActualAmount: number;
    CommitmentAmount: number;
    DoingTickets: number;
    CompletedTickets: number;
  };
  cycleChange(cycle) {
    this.currentCycle = cycle;
    this.loadBudget();
    this.loadAnalytic();
  }

  loadInvestment12Months() {
    this.reportService
      .getReportData(
        btoa(
          JSON.stringify({
            store: "RP_InvestmentYearly",
            storeParams: [],
          })
        )
      )
      .subscribe((response) => {
        this.investment12MonthsData = response.result;

        this.setupInvestment12Months();
      });
  }
  loadAnalytic6Months() {
    this.reportService
      .getReportData(
        btoa(
          JSON.stringify({
            store: "RP_Analytic6Months",
            storeParams: [],
          })
        )
      )
      .subscribe((response) => {
        this.analytic6MonthsData = response.result;

        this.setupAnalytic6Months();
      });
  }
  loadAnalytic() {
    if (this.currentCycle) {
      this.reportService
        .getReportData(
          btoa(
            JSON.stringify({
              store: "RP_Analytic",
              storeParams: [
                {
                  cycleId: this.currentCycle.Id,
                },
              ],
            })
          )
        )
        .subscribe((response) => {
          this.analytic =
            response.result.length > 0
              ? response.result[0]
              : {
                  Shops: 0,
                  KeyShops: 0,
                  TicketShops: 0,
                  InvestAmount: 0,
                  RewardAmount: 0,
                  MaterialAmount: 0,
                  AvgInvestmentAmount: 0,
                  Orders: 0,
                  RewardQuantity: 0,
                  ActualAmount: 0,
                  CommitmentAmount: 0,
                  DoingTickets: 0,
                  CompletedTickets: 0,
                };

          this.setupTicketOverview();
        });
    } else {
      this.analytic = {
        Shops: 0,
        KeyShops: 0,
        TicketShops: 0,
        InvestAmount: 0,
        RewardAmount: 0,
        MaterialAmount: 0,
        AvgInvestmentAmount: 0,
        Orders: 0,
        RewardQuantity: 0,
        ActualAmount: 0,
        CommitmentAmount: 0,
        DoingTickets: 0,
        CompletedTickets: 0,
      };
      this.setupTicketOverview();
    }
  }
  loadBudget() {
    if (this.currentCycle) {
      this.reportService
        .getReportData(
          btoa(
            JSON.stringify({
              store: "RP_Budget",
              storeParams: [
                {
                  cycleId: this.currentCycle.Id,
                },
              ],
            })
          )
        )
        .subscribe((response) => {
          this.budget =
            response.result.length > 0
              ? response.result[0]
              : { AllocateAmount: 0, UsedAmount: 0, RemainAmount: 0 };

          this.setupBudgetTotal();
        });
    } else {
      this.budget = {
        AllocateAmount: 0,
        UsedAmount: 0,
        RemainAmount: 0,
      };
      this.setupBudgetTotal();
    }
  }
  setupInvestment12Months() {
    this.revenueReportChartoptions = {
      chart: {
        height: 230,
        stacked: true,
        type: "bar",
        toolbar: { show: false },
      },
      plotOptions: {
        bar: {
          columnWidth: "17%",
          endingShape: "rounded",
        },
      },
      colors: [colors.solid.primary, colors.solid.warning],
      dataLabels: {
        enabled: false,
      },
      legend: {
        show: false,
      },
      grid: {
        padding: {
          top: -20,
          bottom: -10,
        },
        yaxis: {
          lines: { show: false },
        },
      },
      series: [
        {
          name: "Đầu Tư",
          data: this.investment12MonthsData.map((p) => p.InvestmentAmount),
        },
        {
          name: "Doanh Số",
          data: this.investment12MonthsData.map((p) => -1 * p.ActualAmount),
        },
      ],
      xaxis: {
        categories: this.investment12MonthsData.map((p) => p.YearMonth),
        labels: {
          style: {
            colors: this.$textMutedColor,
            fontSize: "0.86rem",
          },
        },
        axisTicks: {
          show: false,
        },
        axisBorder: {
          show: false,
        },
      },
      yaxis: {
        labels: {
          style: {
            colors: this.$textMutedColor,
            fontSize: "0.86rem",
          },
        },
      },
    };
  }
  setupAnalytic6Months() {
    this.orderBar = {
      chart: {
        height: 70,
        type: "bar",
        stacked: true,
        toolbar: {
          show: false,
        },
      },
      grid: {
        show: false,
        padding: {
          left: 0,
          right: 0,
          top: -15,
          bottom: -15,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: "20%",
          startingShape: "rounded",
          colors: {
            backgroundBarColors: [
              this.$barColor,
              this.$barColor,
              this.$barColor,
              this.$barColor,
              this.$barColor,
              this.$barColor,
            ],
            backgroundBarRadius: 5,
          },
        },
      },
      legend: {
        show: false,
      },
      dataLabels: {
        enabled: false,
      },
      colors: [colors.solid.warning],
      series: [
        {
          name: "2020",
          data: this.analytic6MonthsData.map(p=>p.Orders),
        },
      ],
      xaxis: {
        labels: {
          show: false,
        },
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
      },
      yaxis: {
        show: false,
      },
      tooltip: {
        x: {
          show: false,
        },
      },
    };
    this.rewardBar = {
      chart: {
        height: 70,
        type: "bar",
        stacked: true,
        toolbar: {
          show: false,
        },
      },
      grid: {
        show: false,
        padding: {
          left: 0,
          right: 0,
          top: -15,
          bottom: -15,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: "20%",
          startingShape: "rounded",
          colors: {
            backgroundBarColors: [
               this.$barColor,
               this.$barColor,
               this.$barColor,
               this.$barColor,
               this.$barColor,
               this.$barColor,
            ],
            backgroundBarRadius: 5,
          },
        },
      },
      legend: {
        show: false,
      },
      dataLabels: {
        enabled: false,
      },
      colors: [colors.solid.success],
      series: [
        {
          name: "2020",
          data: this.analytic6MonthsData.map(p=>p.RewardQuantity),
        },
      ],
      xaxis: {
        labels: {
          show: false,
        },
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
      },
      yaxis: {
        show: false,
      },
      tooltip: {
        x: {
          show: false,
        },
      },
    };

    this.investmentLine = {
      chart: {
        height: 70,
        type: "line",
        toolbar: {
          show: false,
        },
        zoom: {
          enabled: false,
        },
      },
      grid: {
        // show: true,
        borderColor: this.$trackBgColor,
        strokeDashArray: 5,
        xaxis: {
          lines: {
            show: true,
          },
        },
        yaxis: {
          lines: {
            show: false,
          },
        },
        padding: {
          // left: 0,
          // right: 0,
          top: -30,
          bottom: -10,
        },
      },
      stroke: {
        width: 3,
      },
      colors: [colors.solid.primary],
      series: [
        {
          data: this.analytic6MonthsData.map(p=>p.InvestmentAmount),
        },
      ],
      markers: {
        size: 2,
        colors: colors.solid.primary,
        strokeColors: colors.solid.primary,
        strokeWidth: 2,
        strokeOpacity: 1,
        strokeDashArray: 0,
        fillOpacity: 1,
        discrete: [
          {
            seriesIndex: 0,
            dataPointIndex: 5,
            fillColor: "#ffffff",
            strokeColor: colors.solid.primary,
            size: 5,
          },
        ],
        shape: "circle",
        radius: 2,
        hover: {
          size: 3,
        },
      },
      xaxis: {
        labels: {
          show: true,
          style: {
            fontSize: "0px",
          },
        },
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
      },
      yaxis: {
        show: false,
      },
      tooltip: {
        x: {
          show: false,
        },
      },
    };
    this.actualLine = {
      chart: {
        height: 70,
        type: "line",
        toolbar: {
          show: false,
        },
        zoom: {
          enabled: false,
        },
      },
      grid: {
        // show: true,
        borderColor: this.$trackBgColor,
        strokeDashArray: 5,
        xaxis: {
          lines: {
            show: true,
          },
        },
        yaxis: {
          lines: {
            show: false,
          },
        },
        padding: {
          // left: 0,
          // right: 0,
          top: -30,
          bottom: -10,
        },
      },
      stroke: {
        width: 3,
      },
      colors: [colors.solid.success],
      series: [
        {
          data: this.analytic6MonthsData.map(p=>p.ActualAmount),
        },
      ],
      markers: {
        size: 2,
        colors: colors.solid.success,
        strokeColors: colors.solid.success,
        strokeWidth: 2,
        strokeOpacity: 1,
        strokeDashArray: 0,
        fillOpacity: 1,
        discrete: [
          {
            seriesIndex: 0,
            dataPointIndex: 5,
            fillColor: "#ffffff",
            strokeColor: colors.solid.success,
            size: 5,
          },
        ],
        shape: "circle",
        radius: 2,
        hover: {
          size: 3,
        },
      },
      xaxis: {
        labels: {
          show: true,
          style: {
            fontSize: "0px",
          },
        },
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
      },
      yaxis: {
        show: false,
      },
      tooltip: {
        x: {
          show: false,
        },
      },
    };
  }
  setupBudgetTotal() {
    var usedPercent = this.budget.AllocateAmount
      ? +((this.budget.UsedAmount / this.budget.AllocateAmount) * 100).toFixed(
          1
        )
      : 0;

    this.budgetTotalChartoptions = {
      chart: {
        type: "donut",
        height: 200,
        toolbar: {
          show: false,
        },
      },
      dataLabels: {
        enabled: false,
      },
      series: [usedPercent, 100 - usedPercent],
      legend: { show: false },
      labels: ["Used", "Remain"],
      stroke: { width: 0 },
      colors: ["#3f51b5", "#5c6bc0"],
      plotOptions: {
        pie: {
          donut: {
            labels: {
              show: true,
              name: {
                offsetY: 15,
              },
              value: {
                offsetY: -15,
                formatter: function (val) {
                  return parseInt(val) + "%";
                },
              },
              total: {
                show: true,
                offsetY: 15,
                label: "Used",
                formatter: function () {
                  return usedPercent + " %";
                },
              },
            },
          },
        },
      },
      responsive: [
        {
          breakpoint: 1325,
          options: {
            chart: {
              height: 100,
            },
          },
        },
        {
          breakpoint: 1200,
          options: {
            chart: {
              height: 120,
            },
          },
        },
        {
          breakpoint: 1065,
          options: {
            chart: {
              height: 100,
            },
          },
        },
        {
          breakpoint: 992,
          options: {
            chart: {
              height: 120,
            },
          },
        },
      ],
    };
  }

  setupTicketOverview() {
    var completedPercent = Math.round(
      (this.analytic.CompletedTickets /
        (this.analytic.DoingTickets + this.analytic.CompletedTickets)) *
        100
    );
    this.goalChartoptions = {
      chart: {
        height: 245,
        type: "radialBar",
        sparkline: {
          enabled: true,
        },
        dropShadow: {
          enabled: true,
          blur: 3,
          left: 1,
          top: 1,
          opacity: 0.1,
        },
      },
      series: [completedPercent],
      colors: [this.$goalStrokeColor2],
      plotOptions: {
        radialBar: {
          offsetY: -10,
          startAngle: -150,
          endAngle: 150,
          hollow: {
            size: "77%",
          },
          track: {
            background: this.$strokeColor,
            strokeWidth: "50%",
          },
          dataLabels: {
            name: {
              show: false,
            },
            value: {
              color: this.$textHeadingColor,
              fontSize: "2.86rem",
              fontWeight: "600",
            },
          },
        },
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "dark",
          type: "horizontal",
          shadeIntensity: 0.5,
          gradientToColors: [colors.solid.success],
          inverseColors: true,
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 100],
        },
      },
      stroke: {
        lineCap: "round",
      },
      grid: {
        padding: {
          bottom: 30,
        },
      },
    };
  }
  /**
   * Constructor
   * @param {AuthenticationService} _authenticationService
   * @param {DashboardService} _dashboardService
   * @param {CoreConfigService} _coreConfigService
   * @param {CoreTranslationService} _coreTranslationService
   */
  constructor(
    injector: Injector,
    private reportService: ReportService,

    private _authenticationService: AuthenticationService,
    private _dashboardService: DashboardService
  ) {
    super(injector);

    this.reportService
      .getReportData(
        btoa(
          JSON.stringify({
            store: "RP_Cycle",
          })
        )
      )
      .subscribe((response) => {
        this.cycles = response.result;
        var current = moment().startOf("day");
        this.currentCycle = this.cycles.find(
          (item) =>
            moment(item.FromDate).startOf("day").isSameOrBefore(current) &&
            moment(item.ToDate).endOf("day").isSameOrAfter(current)
        );
        this.cycleChange(this.currentCycle);
      });
    this._authenticationService.currentUser$.subscribe(
      (x) => (this.currentUser = x)
    );

    this.loadInvestment12Months();
    this.loadAnalytic6Months();
    this.setupAnalytic6Months();
    this.setupInvestment12Months();
    // Statistics Bar Chart
  
    // Statistics Line Chart
    
    // Budget Chart
    this.budgetChartoptions = {
      chart: {
        height: 80,
        toolbar: { show: false },
        zoom: { enabled: false },
        type: "line",
        sparkline: { enabled: true },
      },
      stroke: {
        curve: "smooth",
        dashArray: [0, 5],
        width: [2],
      },
      colors: [colors.solid.primary, this.$budgetStrokeColor2],
      tooltip: {
        enabled: false,
      },
    };

    // Browser States Primary Chart
    this.statePrimaryChartoptions = {
      chart: {
        height: 30,
        width: 30,
        type: "radialBar",
      },
      grid: {
        show: false,
        padding: {
          left: -15,
          right: -15,
          top: -12,
          bottom: -15,
        },
      },
      colors: [colors.solid.primary],
      series: [54.4],
      plotOptions: {
        radialBar: {
          hollow: {
            size: "22%",
          },
          track: {
            background: this.$trackBgColor,
          },
          dataLabels: {
            showOn: "always",
            name: {
              show: false,
            },
            value: {
              show: false,
            },
          },
        },
      },
      stroke: {
        lineCap: "round",
      },
    };

    // Browser States Warning Chart
    this.stateWarningChartoptions = {
      chart: {
        height: 30,
        width: 30,
        type: "radialBar",
      },
      grid: {
        show: false,
        padding: {
          left: -15,
          right: -15,
          top: -12,
          bottom: -15,
        },
      },
      colors: [colors.solid.warning],
      series: [6.1],
      plotOptions: {
        radialBar: {
          hollow: {
            size: "22%",
          },
          track: {
            background: this.$trackBgColor,
          },
          dataLabels: {
            showOn: "always",
            name: {
              show: false,
            },
            value: {
              show: false,
            },
          },
        },
      },
      stroke: {
        lineCap: "round",
      },
    };

    // Browser States Secondary Chart
    this.stateSecondaryChartoptions = {
      chart: {
        height: 30,
        width: 30,
        type: "radialBar",
      },
      grid: {
        show: false,
        padding: {
          left: -15,
          right: -15,
          top: -12,
          bottom: -15,
        },
      },
      colors: [colors.solid.secondary],
      series: [14.6],
      plotOptions: {
        radialBar: {
          hollow: {
            size: "22%",
          },
          track: {
            background: this.$trackBgColor,
          },
          dataLabels: {
            showOn: "always",
            name: {
              show: false,
            },
            value: {
              show: false,
            },
          },
        },
      },
      stroke: {
        lineCap: "round",
      },
    };

    // Browser States Info Chart
    this.stateInfoChartoptions = {
      chart: {
        height: 30,
        width: 30,
        type: "radialBar",
      },
      grid: {
        show: false,
        padding: {
          left: -15,
          right: -15,
          top: -12,
          bottom: -15,
        },
      },
      colors: [colors.solid.info],
      series: [4.2],
      plotOptions: {
        radialBar: {
          hollow: {
            size: "22%",
          },
          track: {
            background: this.$trackBgColor,
          },
          dataLabels: {
            showOn: "always",
            name: {
              show: false,
            },
            value: {
              show: false,
            },
          },
        },
      },
      stroke: {
        lineCap: "round",
      },
    };

    // Browser States Danger Chart
    this.stateDangerChartoptions = {
      chart: {
        height: 30,
        width: 30,
        type: "radialBar",
      },
      grid: {
        show: false,
        padding: {
          left: -15,
          right: -15,
          top: -12,
          bottom: -15,
        },
      },
      colors: [colors.solid.danger],
      series: [8.4],
      plotOptions: {
        radialBar: {
          hollow: {
            size: "22%",
          },
          track: {
            background: this.$trackBgColor,
          },
          dataLabels: {
            showOn: "always",
            name: {
              show: false,
            },
            value: {
              show: false,
            },
          },
        },
      },
      stroke: {
        lineCap: "round",
      },
    };
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    // get the currentUser details from localStorage
    this.currentUser = JSON.parse(localStorage.getItem("currentUser"));

    // Get the dashboard service data
    this._dashboardService.onApiDataChanged.subscribe((response) => {
      this.data = response;
    });
  }

  getNumber(value: number) {
    if (value) {
      return formatNumber(value, "#,##0.##");
    }
    return 0;
  }
}
