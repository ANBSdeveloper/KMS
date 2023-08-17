//#region Import
import { Component, Injector, Input, ViewEncapsulation } from "@angular/core";
import { formatNumber } from "devextreme/localization";
import { PermissionComponentBase } from "@cbms/ng-core-vuexy";
import {
  DataServiceProxy,
  PosmInvestmentDto,
  PosmInvestmentItemHistoryDto,
  PosmInvestmnetItemHistoryItemDto,
} from "@shared/services/data.service";
import moment from "moment";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { DialogService } from "primeng/dynamicdialog";
import { Observable, of } from "rxjs";
import { finalize, map } from "rxjs/operators";
import { PosmInvestmentItemStatusDataSource } from "../../data-source/posm-investment-item-status.data-source";
import { PosmInvestmentItemStatus } from "../../data-source/posm-investment-status.enum";
//#endregion

@Component({
  selector: "app-posm-investment-history",
  templateUrl: "./posm-investment-history.component.html",
  styleUrls: ["./posm-investment-history.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class PosmInvestmentHistoryComponent extends PermissionComponentBase {
  @Input() set investment(value) {
    this._investment = value;
    this.refresh();
  }
  @Input() status: PosmInvestmentItemStatus[];
  @BlockUI("posm_investment_history_content_block") blockUI: NgBlockUI;
  _investment: PosmInvestmentDto;
  items: PosmInvestmnetItemHistoryItemDto[] = [];
  get investment() {
    return this._investment;
  }
  @Input() set posmInvestmentItemId(value) {
    this._posmInvestmentItemId = value;
    this.refresh();
  }

  get posmInvestmentItemId() {
    return this._posmInvestmentItemId;
  }
  _posmInvestmentItemId: number;
  constructor(
    injector: Injector,
    private dataService: DataServiceProxy,
    private posmInvestmentItemStatusDataSource: PosmInvestmentItemStatusDataSource,
    private dialogService: DialogService
  ) {
    super(injector);
  }

  cache = {};
  cacheStaff = {};
  cacheLocation = {};
  cacheCustomer = {};

  refresh() {
    if (this.investment?.items?.length > 0) {
      this.blockUI.start();
      this.dataService
        .getPosmInvestmentItemHistory(
          this.posmInvestmentItemId
            ? this.posmInvestmentItemId
            : this.investment.items[0].id
        )
        .pipe(finalize(() => this.blockUI.stop()))
        .subscribe((response) => {
          this.items = response.result.requestItems
            .concat(response.result.approveItems)
            .concat(response.result.prepareItems)
            .concat(response.result.operationItems)
            .concat(response.result.acceptanceItems)
            .filter((p) => this.status.indexOf(<any>p.status) != -1);
        });
    } else {
      this.items = [];
    }
  }

  getData(item: PosmInvestmnetItemHistoryItemDto) {
    if (!this.cache[item.id]) {
      this.cache[item.id] = JSON.parse(item.data);
    }
    return this.cache[item.id];
  }

  formatNum(value) {
    return formatNumber(value, "#,###.##");
  }
  // getStaff(id): Observable<UserDto> {
  //   if (!id) return of(new UserDto());
  //   if (!this.cacheStaff[id]) {
  //     this.cacheStaff[id] = this.dataService.getUser(id).pipe(
  //       filter((item) => item != undefined),
  //       map((item) => item.result.name)
  //     );
  //   }
  //   return this.cacheStaff[id];
  // }

  // getLocation(id): Observable<LocationDto> {
  //   if (!id) return of(new LocationDto());
  //   if (!this.cacheLocation[id]) {
  //     this.cacheLocation[id] = this.dataService.getLocation(id).pipe(
  //       filter((item) => item != undefined),
  //       map((item) => item.result.name)
  //     );
  //   }
  //   return this.cacheLocation[id];
  // }

  getCustomer(id): Observable<string> {
    if (!id) return of("");
    if (!this.cacheCustomer[id]) {
      this.cacheCustomer[id] = this.dataService
        .getCustomer(id)
        .pipe(map((item) => item.result.name));
    }
    return this.cacheCustomer[id];
  }

  getTimePointCss(item) {
    var status = item.status;
    if (
      status == PosmInvestmentItemStatus.AsmApprovedRequest ||
      status == PosmInvestmentItemStatus.RsmApprovedRequest ||
      status == PosmInvestmentItemStatus.TradeApprovedRequest ||
      status == PosmInvestmentItemStatus.DirectorApprovedRequest ||
      status == PosmInvestmentItemStatus.AsmConfirmedUpdateCost ||
      status == PosmInvestmentItemStatus.RsmConfirmedUpdateCost ||
      status == PosmInvestmentItemStatus.TradeConfirmedUpdateCost ||
      status == PosmInvestmentItemStatus.Accepted ||
      status == PosmInvestmentItemStatus.ConfirmedAccept1 ||
      status == PosmInvestmentItemStatus.ConfirmedAccept2 ||
      status == PosmInvestmentItemStatus.ConfirmedProduce1 ||
      status == PosmInvestmentItemStatus.ConfirmedProduce2 ||
      status == PosmInvestmentItemStatus.ConfirmedVendorProduce
    )
      return "timeline-point-success";
    if (
      status == PosmInvestmentItemStatus.AsmDeniedRequest ||
      status == PosmInvestmentItemStatus.RsmDeniedRequest ||
      status == PosmInvestmentItemStatus.TradeDeniedRequest ||
      status == PosmInvestmentItemStatus.DirectorDeniedRequest
    )
      return "timeline-point-warning";
    if (status == PosmInvestmentItemStatus.InvalidOrder)
      return "timeline-point-warning";
    if (status == PosmInvestmentItemStatus.ValidOrder)
      return "timeline-point-success";
    if (
      status == PosmInvestmentItemStatus.RsmDeniedRequest ||
      status == PosmInvestmentItemStatus.DirectorDeniedRequest ||
      status == PosmInvestmentItemStatus.AsmDeniedRequest
    )
      return "timeline-point-danger";
  }

  getTitle(data) {
    return this.posmInvestmentItemStatusDataSource.getItemName(data.Status);
  }

  // isPicking(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.DeliveryConfirmedPicking;
  // }

  // isAssign(item: PosmInvestmentHistoryDto): boolean {
  //   return (
  //     this.getData(item).status == OrderStatus.WarehouseTransferedToDelivery
  //   );
  // }

  // isConfirmAssign(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.DeliveryManReceivedItems;
  // }

  // isStartDelivery(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.DeliveryManDelivery;
  // }

  // isCancelReceiveDelivery(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.CustomerNoReceive;
  // }

  // isReturnShipping(item: PosmInvestmentHistoryDto): boolean {
  //   return (
  //     this.txnType == OrderTransactionType.ShippingOrder &&
  //     this.getData(item).status == OrderStatus.DelveryManConfirmedReturn
  //   );
  // }

  // isReturnBackOrder(item: PosmInvestmentHistoryDto): boolean {
  //   return (
  //     this.txnType == OrderTransactionType.BackOrder &&
  //     this.getData(item).status == OrderStatus.DelveryManConfirmedReturn
  //   );
  // }

  // isReceiveReturnShipping(item: PosmInvestmentHistoryDto): boolean {
  //   return (
  //     this.txnType == OrderTransactionType.ShippingOrder &&
  //     this.getData(item).status == OrderStatus.WarehouseReceivedReturn
  //   );
  // }

  // isReceiveReturnBackOrder(item: PosmInvestmentHistoryDto): boolean {
  //   return (
  //     this.txnType == OrderTransactionType.BackOrder &&
  //     this.getData(item).status == OrderStatus.WarehouseReceivedReturn
  //   );
  // }

  // isCompleteReturnShipping(item: PosmInvestmentHistoryDto): boolean {
  //   return (
  //     this.txnType == OrderTransactionType.ShippingOrder &&
  //     this.getData(item).status == OrderStatus.WarehouseCompletedReturn
  //   );
  // }

  // isCanceled(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.Canceled;
  // }

  // isRescheduleDelivery(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.DeliveryRescheduled;
  // }

  // isCompleteDelivery(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.DeliveryManDeliveried;
  // }

  // isSettle(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.DeliveryManSettled;
  // }

  // isAggregateSettle(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.AccountCreatedBatchOrder;
  // }

  // isAccountantConfirmSettle(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.AccountantConfirmed;
  // }

  // isStartRecall(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.ReturnOrderHolding;
  // }

  // isRecall(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.CustomerReturned;
  // }

  // isStartCollectDebt(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.ReceivableOrderHolding;
  // }

  // isReceiveReceivable(item: PosmInvestmentHistoryDto): boolean {
  //   return this.getData(item).status == OrderStatus.ReceivableOrderReceived;
  // }

  isRegister(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return <any>item.status == PosmInvestmentItemStatus.Request;
  }

  isApproval(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return (
      <any>item.status == PosmInvestmentItemStatus.AsmApprovedRequest ||
      <any>item.status == PosmInvestmentItemStatus.RsmApprovedRequest || 
      <any>item.status == PosmInvestmentItemStatus.TradeApprovedRequest || 
      <any>item.status == PosmInvestmentItemStatus.DirectorApprovedRequest 
    );
  }
  isDeny(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return (
      <any>item.status == PosmInvestmentItemStatus.AsmDeniedRequest ||
      <any>item.status == PosmInvestmentItemStatus.RsmDeniedRequest ||
      <any>item.status == PosmInvestmentItemStatus.DirectorDeniedRequest ||
      <any>item.status == PosmInvestmentItemStatus.TradeDeniedRequest
    );
  }
  isInvalid(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return <any>item.status == PosmInvestmentItemStatus.InvalidOrder;
  }
  isValid(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return <any>item.status == PosmInvestmentItemStatus.ValidOrder;
  }
  isConfirmSuggest(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return (
      <any>item.status >= PosmInvestmentItemStatus.SupSuggestedUpdateCost &&
      <any>item.status <= PosmInvestmentItemStatus.TradeConfirmedUpdateCost
    );
  }

  isOperation(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return (
      <any>item.status >= PosmInvestmentItemStatus.ConfirmedProduce1 &&
      <any>item.status <= PosmInvestmentItemStatus.ConfirmedVendorProduce
    );
  }

  isAcceptance(item: PosmInvestmnetItemHistoryItemDto): boolean {
    return (
      <any>item.status >= PosmInvestmentItemStatus.Accepted &&
      <any>item.status <= PosmInvestmentItemStatus.ConfirmedAccept2
    );
  }
  // showPicking(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderPickingDialogComponent, {
  //     data: {
  //       picking: this.posmInvestment.picking,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_picking_title"),
  //   });
  // }

  // showAssign(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderAssignDialogComponent, {
  //     data: {
  //       assignment: this.posmInvestment.assignment,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_assign_title"),
  //   });
  // }

  // showConfirmAssign(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderConfirmAssignDialogComponent, {
  //     data: {
  //       assignment: this.posmInvestment.assignment,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_confirm_assign_title"),
  //   });
  // }

  // showStartDelivery(e, historyOrder: OrderDto) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderStartDeliveryDialogComponent, {
  //     data: {
  //       delivery: historyOrder.delivery,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_start_delivery_title"),
  //   });
  // }

  // showCancelReceiveDelivery(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderCancelReceiveDeliveryDialogComponent, {
  //     data: {
  //       delivery: this.posmInvestment.delivery,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     width: "400px",
  //     header: this.l("order_cancel_receive_delivery_title"),
  //   });
  // }

  // showReturnShipping(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderReturnShippingDialogComponent, {
  //     data: {
  //       return: this.posmInvestment.return,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_return_shipping_title"),
  //   });
  // }

  // showReturnBackOrder(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderReturnBackOrderDialogComponent, {
  //     data: {
  //       return: this.posmInvestment.return,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_return_backorder_title"),
  //   });
  // }

  // showReceiveReturnShipping(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderReceiveReturnShippingDialogComponent, {
  //     data: {
  //       return: this.posmInvestment.return,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_receive_return_shipping_title"),
  //   });
  // }
  // showReceiveReturnBackOrder(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderReceiveBackOrderDialogComponent, {
  //     data: {
  //       return: this.posmInvestment.return,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_receive_return_backorder_title"),
  //   });
  // }
  // showRescheduleDelivery(e, historyOrder) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderRescheduleDeliveryDialogComponent, {
  //     data: {
  //       rescheduleDelivery:
  //         historyOrder.rescheduleDeliveries[
  //           historyOrder.rescheduleDeliveries.length - 1
  //         ],
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_reschedule_delivery_title"),
  //   });
  // }

  // showCompleteDelivery(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderCompleteDeliveryDialogComponent, {
  //     data: {
  //       delivery: this.posmInvestment.delivery,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_complete_delivery_title"),
  //   });
  // }
  // showSettle(e) {
  //   e.preventDefault();
  //   this.dialogService.open(OrderSettleDialogComponent, {
  //     data: {
  //       settlement: this.posmInvestment.settlement,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("order_settle_title"),
  //   });
  // }
  // showRecall(e) {
  //   e.preventDefault();
  //   this.dialogService.open(BackOrderRecallDialogComponent, {
  //     data: {
  //       return: this.posmInvestment.return,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     width: "600px",
  //     header: this.l("back_order_recall_title"),
  //   });
  // }

  // showReceiveReceivable(e) {
  //   e.preventDefault();
  //   this.dialogService.open(ReceivableOrderReceiveDialogComponent, {
  //     data: {
  //       receivable: this.posmInvestment.receivable,
  //       orderId: this.posmInvestment.id,
  //       order: this.posmInvestment,
  //       readOnly: true,
  //     },
  //     baseZIndex: 100,
  //     header: this.l("receivable_order_receive_title"),
  //   });
  // }
  toDate(value) {
    return moment(value).toDate();
  }
}
