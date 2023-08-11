import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { CoreCommonModule } from "@core/common.module";
import { Ng2ImgMaxModule } from "ng2-img-max";
import { ImageViewerComponent } from "./components/image-viewer/image-viewer.component";
import { PagerComponent } from "./components/pager/pager.component";
import { CbmsPipesModule } from "./pipes/pipes.module";
import { DataServiceProxy } from "./services/data.service";
import {DialogModule} from 'primeng/dialog';
@NgModule({
  declarations: [PagerComponent, ImageViewerComponent],
  imports: [DialogModule, CoreCommonModule, CbmsPipesModule, CommonModule, Ng2ImgMaxModule],
  exports: [PagerComponent, ImageViewerComponent, CbmsPipesModule],
  providers: [DataServiceProxy],
})
export class SharedModule {}
