import { NgModule } from "@angular/core";

import { DateFormatPipe } from "./date-format.pipe";
import { NumberFormatPipe } from "./number-format.pipe";

@NgModule({
  declarations: [DateFormatPipe, NumberFormatPipe],
  imports: [],
  exports: [DateFormatPipe, NumberFormatPipe],
})
export class CbmsPipesModule {}
