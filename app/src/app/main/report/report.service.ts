// Angular
import {
	Inject,
	Injectable} from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { API_BASE_URL } from "@shared/services/data.service";
import { environment } from "environments/environment";

@Injectable()
export class ReportService {
	constructor(
		public http: HttpClient,
		@Inject(API_BASE_URL) baseUrl
	) {
	}

	getReportData(params: string): Observable<any> {
		let httpParams = new HttpParams();

		httpParams = httpParams.append("params", params);
		var url = environment.apiUrl + "/api/v1/report";
		return this.http.get<any[]>(url, {
			params: httpParams
		});
	}

	openReport(title: string,name: string, params: string) {
		var base64 = btoa(params);
		window.open(
			`${document.baseURI}report/viewer?name=${name}&params=${base64}&title=${title}`,
			"_blank"
		);
	}
}
