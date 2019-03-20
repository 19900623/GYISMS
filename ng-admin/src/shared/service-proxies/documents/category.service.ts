import { HttpClient } from "@angular/common/http";
import { GyismsHttpClient } from "@shared/service-proxies/gyisms-httpclient";
import { Injectable, Inject, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { Observable } from "rxjs";
import { ApiResult } from "../entity/parameter";

@Injectable()
export class CategoryService {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }

    createOrUpdate(input: any): Observable<any> {
        let url_ = "/api/services/app/DocCategory/CreateOrUpdate";
        let cat = { docCategory: input };
        return this._gyhttp.post(url_, cat).map(data => {
            return data;
        });
    }

    getTreeAsync(deptId: any): Observable<any> {
        let url_ = "/api/services/app/DocCategory/GetTreeAsync";
        return this._gyhttp.get(url_, { deptId: deptId }).map(data => {
            return data;
        });
    }

    getParentName(id: number): Observable<string> {
        let url_ = "/api/services/app/DocCategory/GetParentName?id=" + id;
        return this._gyhttp.get(url_).map(data => {
            return data;
        });
    }

    deleteCategoryById(id: number): Observable<ApiResult> {
        let url_ = "/api/services/app/DocCategory/CategoryRemoveById";
        var param = { id: id };
        return this._gyhttp.post(url_, param).map(data => {
            return data;
        });
    }

    copyCategoryByDeptId(input: any): Observable<ApiResult> {
        let url_ = "/api/services/app/DocCategory/CopyCategoryByDeptIdAsync";
        return this._gyhttp.post(url_, input).map(data => {
            return data;
        });
    }

    getCopyTreeWithRootAsync(deptId: any): Observable<any> {
        let url_ = "/api/services/app/DocCategory/GetCopyTreeWithRootAsync";
        return this._gyhttp.get(url_, { deptId: deptId }).map(data => {
            return data;
        });
    }
}
