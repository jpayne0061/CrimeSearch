"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SearchParameter = void 0;
var SearchParameter = /** @class */ (function () {
    function SearchParameter(fieldName, operator, searchValue, fieldType, displayName) {
        this.fieldName = fieldName;
        this.searchOperator = operator;
        this.searchValue = searchValue;
        this.fieldType = fieldType;
        this.displayName = displayName;
    }
    return SearchParameter;
}());
exports.SearchParameter = SearchParameter;
//# sourceMappingURL=searchParameter.js.map