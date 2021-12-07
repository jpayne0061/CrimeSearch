import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SearchParameter } from '../../models/searchParameter';
import { CrimeInstance } from '../../models/crimeInstance';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public crimes: CrimeInstance[];

  public searchParameters: SearchParameter[] = [
    new SearchParameter("datE_OCCURED", ">", "", "datetime", "Date Time Occurred"),
    new SearchParameter("datE_REPORTED", ">", "", "datetime", "Date Time Reported"),
    new SearchParameter("uoR_DESC", "=", "", "string", "UOR Description"),
    new SearchParameter("crimE_TYPE", "=", "", "string", "Crime Type"),
    new SearchParameter("ziP_CODE", "=", "", "string", "Zip Code")
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

      this.crimes = null;

      search.push(this.searchParameters[i]);
    }


    this.http.post<CrimeInstance[]>(this.baseUrl + 'crimesearch', search).subscribe(result => {
      console.log("crimes: ", result);
      this.crimes = result;
    }, error => console.error(error));
  }

  public listTopFive() {
    this.http.get<CrimeInstance[]>(this.baseUrl + 'crimesearch').subscribe(result => {
      console.log("crimes: ", result);
      this.crimes = result;
    }, error => alert(error));
  }

}
