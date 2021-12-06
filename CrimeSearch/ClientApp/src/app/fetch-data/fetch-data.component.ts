import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public crimes: CrimeInstance[];

  public searchValue: string;
  private baseUrl;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public search() {
    this.http.post<CrimeInstance[]>(this.baseUrl + 'weatherforecast', { fieldName: "ZIP_CODE", fieldValue: this.searchValue }).subscribe(result => {
      console.log("crimes: ", result);
      this.crimes = result;
    }, error => console.error(error));
  }

}

export class CrimeInstance {
  public incidenT_NUMBER: string;
  public datE_REPORTED: Date;
  public datE_OCCURED: Date;
  public uoR_DESC: string;
  public crimE_TYPE: string;
  public nibrS_CODE: string;
  public ucR_HIERARCHY: string;
  public atT_COMP: string;
  public lmpD_DIVISION: string;
  public lmpD_BEAT: string;
  public premisE_TYPE: string;
  public blocK_ADDRESS: string;
  public city: string;
  public ziP_CODE: string;
}


