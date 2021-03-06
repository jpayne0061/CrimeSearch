export class SearchParameter {
  public fieldName: string;
  public searchOperator: string;
  public availableOperators: string[];
  public searchValue: string;
  public fieldType: string;
  public displayName: string;

  public constructor(
      fieldName: string,
      operator: string,
      searchValue: string,
      availableOperators: string[],
      fieldType: string,
      displayName: string) {
    this.fieldName = fieldName;
    this.searchOperator = operator;
    this.searchValue = searchValue;
    this.fieldType = fieldType;
    this.displayName = displayName;
    this.availableOperators = availableOperators;
  }
}
