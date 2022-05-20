import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SearchParameter } from '../../models/searchParameter';
import { CrimeInstance } from '../../models/crimeInstance';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public crimes: any[];

  public objectKeys = [];

  public searchParameters: SearchParameter[] = [
    new SearchParameter("datE_OCCURED", ">", "", ["<","<=", ">", ">="], "datetime", "Date Time Occurred"),
    new SearchParameter("datE_REPORTED", ">", "", ["<", "<=", ">", ">="], "datetime", "Date Time Reported"),
    new SearchParameter("uoR_DESC", "=", "", ["=", "!="], "string", "UOR Description"),
    new SearchParameter("crimE_TYPE", "=", "", ["=", "!="], "string", "Crime Type"),
    new SearchParameter("ziP_CODE", "=", "", ["=", "!="], "string", "Zip Code")
  ];

  private baseUrl;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.listTopFive()
  }

  public search() {
    

    let search: SearchParameter[] = [];

    for (let i = 0; i < this.searchParameters.length; i++) {
      if (!this.searchParameters[i].searchValue) {
        continue;
      }

      if (!this.searchParameters[i].searchOperator) {
        alert("operator is missing for " + this.searchParameters[i].displayName + ". Please select >, <, =, or != from the dropdown.");
        return;
      }

      search.push(this.searchParameters[i]);
    }

    this.crimes = null;

    this.http.post<any[]>(this.baseUrl + 'crimesearch', search).subscribe(result => {
      console.log("crimes: ", result);
      this.crimes = result;
    }, error => console.error(error));
  }

  public listTopFive() {
    this.http.get<any[]>(this.baseUrl + 'crimesearch').subscribe(result => {
      this.objectKeys = Object.keys(result[0]);

      console.log("keys: ", this.objectKeys);
      console.log("crimes: ", result);
      this.crimes = result;
    }, error => alert(error));
  }

}
