import { Pipe, PipeTransform } from "@angular/core";
import { formatNumber } from "devextreme/localization";
@Pipe({
  name: "numberFormat",
})
export class NumberFormatPipe implements PipeTransform {
  /**
   * Transform
   *
   * @param {any[]} items
   * @param {string} searchText
   * @param {string} key
   *
   * @returns {any}
   */
  transform(value: number, format: string = "#,##0.##"): string {
    if (value !== undefined && value !== null) {
      return formatNumber(value, format);
    }
    return null;
  }
}
