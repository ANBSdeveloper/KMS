import { Pipe, PipeTransform } from "@angular/core";
import { formatDate } from "devextreme/localization";
@Pipe({
  name: "dateFormat",
})
export class DateFormatPipe implements PipeTransform {
  /**
   * Transform
   *
   * @param {any[]} items
   * @param {string} searchText
   * @param {string} key
   *
   * @returns {any}
   */
  transform(value: Date, format: string = "shortDate"): string {
    if (value) {
      return formatDate(value, format);
    }
    return null;
  }
}
