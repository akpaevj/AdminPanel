var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
function getUrl(url) {
    var path = window.location.pathname;
    if (path != '' && path != '/' && path != null) {
        return path + "/" + url;
    }
    return url;
}
var Common;
(function (Common) {
    var ItemViewModel = /** @class */ (function () {
        function ItemViewModel() {
        }
        return ItemViewModel;
    }());
    Common.ItemViewModel = ItemViewModel;
    function setAddButtonAvailability(select, addButton) {
        if ($(select).children().length > 0) {
            $(addButton).removeClass('disabled')
                .prop('disabled', false);
        }
        else {
            $(addButton).addClass('disabled')
                .prop('disabled', true);
        }
    }
    function removeOption(select, addButton, value) {
        $(select).children('option').each(function (index, elem) {
            if ($(elem).val() == value) {
                $(elem).remove();
            }
        });
        setAddButtonAvailability(select, addButton);
    }
    function addOption(select, addButton, text, value) {
        var option = document.createElement('option');
        $(option).val(value)
            .text(text)
            .appendTo(select);
        setAddButtonAvailability(select, addButton);
    }
    function removeRow(select, table, addButton, deleteButton) {
        var text;
        var value;
        var tr = $(deleteButton).closest('tr');
        var idInput = $(tr).find(':input:hidden').first();
        value = idInput.val();
        text = idInput.closest('td').text();
        tr.remove();
        addOption(select, addButton, text, value);
    }
    function addRow(select, table, addButton) {
        var selectedIndex = $(select).prop('selectedIndex');
        var selectedItem = $(select).children().eq(selectedIndex);
        var value = $(selectedItem).val();
        var label = $(selectedItem).text();
        var tbody = $(table).find('tbody');
        var tableId = $(table).attr('id');
        var row = document.createElement('tr');
        $(row).appendTo(tbody);
        var col1 = document.createElement('td');
        $(col1).text(label)
            .appendTo(row);
        var inputName = document.createElement('input');
        $(inputName).val(value)
            .attr('type', 'hidden')
            .attr('name', "" + tableId)
            .appendTo(col1);
        var col2 = document.createElement('td');
        $(col2).appendTo(row);
        var deleteBtn = document.createElement('button');
        $(deleteBtn).attr('type', 'button')
            .addClass('btn btn-danger')
            .click(function (event) {
            removeRow(select, table, addButton, event.currentTarget);
        })
            .appendTo(col2);
        var deleteI = document.createElement('i');
        $(deleteI).addClass('fa fa-trash')
            .appendTo(deleteBtn);
        removeOption(select, addButton, value);
    }
    function connectSelectToTable(select, addButton, table) {
        setAddButtonAvailability(select, addButton);
        $(addButton).click(function () {
            addRow(select, table, addButton);
        });
        $(table).find('.fa-trash').each(function (index, elem) {
            $(elem).closest('button').click(function () {
                return removeRow(select, table, addButton, elem);
            });
        });
    }
    Common.connectSelectToTable = connectSelectToTable;
})(Common || (Common = {}));
var InfoBases;
(function (InfoBases) {
    var InfoBaseItemViewModel = /** @class */ (function (_super) {
        __extends(InfoBaseItemViewModel, _super);
        function InfoBaseItemViewModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return InfoBaseItemViewModel;
    }(Common.ItemViewModel));
    InfoBases.InfoBaseItemViewModel = InfoBaseItemViewModel;
    var InfoBaseViewModel = /** @class */ (function () {
        function InfoBaseViewModel() {
        }
        return InfoBaseViewModel;
    }());
    InfoBases.InfoBaseViewModel = InfoBaseViewModel;
    var InfoBaseIndexViewModel = /** @class */ (function () {
        function InfoBaseIndexViewModel() {
        }
        return InfoBaseIndexViewModel;
    }());
    InfoBases.InfoBaseIndexViewModel = InfoBaseIndexViewModel;
    function showFormGroupFor(elem) {
        $(elem).closest('.form-group')
            .show();
    }
    function hideFormGroupFor(elem) {
        $(elem).val('')
            .closest('.form-group')
            .hide();
    }
    function setGroupsVisibility() {
        var select = $('#ConnectionType');
        var selectedIndex = $(select).prop('selectedIndex');
        var value = $(select).children().eq(selectedIndex).val();
        switch (value) {
            case "0":
                showFormGroupFor($('#Path'));
                hideFormGroupFor($('#URL'));
                hideFormGroupFor($('#Server'));
                hideFormGroupFor($('#InfoBaseName'));
                break;
            case "1":
                hideFormGroupFor($('#Path'));
                hideFormGroupFor($('#URL'));
                showFormGroupFor($('#Server'));
                showFormGroupFor($('#InfoBaseName'));
                break;
            case "2":
                hideFormGroupFor($('#Path'));
                showFormGroupFor($('#URL'));
                hideFormGroupFor($('#Server'));
                hideFormGroupFor($('#InfoBaseName'));
                break;
            default:
                break;
        }
    }
    function initInfoBaseCreateEditView() {
        setGroupsVisibility();
        $('#ConnectionType').change(function (e, t) {
            setGroupsVisibility();
        });
    }
    InfoBases.initInfoBaseCreateEditView = initInfoBaseCreateEditView;
})(InfoBases || (InfoBases = {}));
var InfoBasesLists;
(function (InfoBasesLists) {
    var InfoBasesListItemViewModel = /** @class */ (function (_super) {
        __extends(InfoBasesListItemViewModel, _super);
        function InfoBasesListItemViewModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return InfoBasesListItemViewModel;
    }(Common.ItemViewModel));
    InfoBasesLists.InfoBasesListItemViewModel = InfoBasesListItemViewModel;
    var InfoBasesListViewModel = /** @class */ (function () {
        function InfoBasesListViewModel() {
        }
        return InfoBasesListViewModel;
    }());
    InfoBasesLists.InfoBasesListViewModel = InfoBasesListViewModel;
    var InfoBasesListIndexViewModel = /** @class */ (function () {
        function InfoBasesListIndexViewModel() {
        }
        return InfoBasesListIndexViewModel;
    }());
    InfoBasesLists.InfoBasesListIndexViewModel = InfoBasesListIndexViewModel;
})(InfoBasesLists || (InfoBasesLists = {}));
var Users;
(function (Users) {
    var UserViewModel = /** @class */ (function () {
        function UserViewModel() {
        }
        return UserViewModel;
    }());
    Users.UserViewModel = UserViewModel;
    var UserIndexViewModel = /** @class */ (function () {
        function UserIndexViewModel() {
        }
        return UserIndexViewModel;
    }());
    Users.UserIndexViewModel = UserIndexViewModel;
    function updateFromActiveDirectory() {
        return __awaiter(this, void 0, void 0, function () {
            var btn, response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        btn = $('#updateFromAd');
                        btn.addClass('disabled')
                            .prop('disabled', true);
                        return [4 /*yield*/, fetch('/Users/UpdateFromActiveDirectory')];
                    case 1:
                        response = _a.sent();
                        if (response.ok) {
                            location.reload();
                        }
                        else {
                            alert('Не удалось обновить список пользователей');
                        }
                        btn.removeClass('disabled')
                            .prop('disabled', false);
                        return [2 /*return*/];
                }
            });
        });
    }
    Users.updateFromActiveDirectory = updateFromActiveDirectory;
    function initIndexView() {
        return __awaiter(this, void 0, void 0, function () {
            var url, time;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        url = getUrl('Users/GetUsers');
                        return [4 /*yield*/, getUsers(url, 1, '')];
                    case 1:
                        _a.sent();
                        time = 0;
                        $('#searchBox').keyup(function (event) {
                            clearTimeout(time);
                            time = window.setTimeout(function () {
                                return __awaiter(this, void 0, void 0, function () {
                                    var term;
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0:
                                                term = $('#searchBox').val();
                                                return [4 /*yield*/, getUsers(url, 1, term)];
                                            case 1:
                                                _a.sent();
                                                return [2 /*return*/];
                                        }
                                    });
                                });
                            }, 500);
                        });
                        return [2 /*return*/];
                }
            });
        });
    }
    Users.initIndexView = initIndexView;
    function getUsers(url, pageIndex, term) {
        return __awaiter(this, void 0, void 0, function () {
            var urlParams, response, data;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        urlParams = url + "?pageIndex=" + pageIndex;
                        if (term != '')
                            urlParams += "&term=" + term;
                        return [4 /*yield*/, fetch(urlParams)];
                    case 1:
                        response = _a.sent();
                        if (!response.ok) return [3 /*break*/, 3];
                        return [4 /*yield*/, response.json()];
                    case 2:
                        data = _a.sent();
                        fillUsersTable(data);
                        fillUsersPagesNav(data, url, term);
                        return [3 /*break*/, 4];
                    case 3:
                        alert('Не удалось получить список пользователей');
                        _a.label = 4;
                    case 4: return [2 /*return*/];
                }
            });
        });
    }
    function fillUsersTable(userIndexViewModel) {
        var tbody = $('#dataTable').children('tbody').first().empty();
        userIndexViewModel.items.forEach(function (value) {
            var tr = document.createElement('tr');
            $(tr).appendTo(tbody);
            var tdName = document.createElement('td');
            $(tdName).text(value.name)
                .appendTo(tr);
            var tdSamAccountName = document.createElement('td');
            $(tdSamAccountName).text(value.samAccountName)
                .appendTo(tr);
            var tdInfoBasesListName = document.createElement('td');
            $(tdInfoBasesListName).appendTo(tr);
            if (value.infoBasesList != null)
                $(tdInfoBasesListName).text(value.infoBasesList.name);
            var tdActions = document.createElement('td');
            $(tdActions).appendTo(tr);
            var aEdit = document.createElement('a');
            $(aEdit).addClass('btn btn-secondary col-auto m-1')
                .attr('href', "/Users/Edit/" + value.id)
                .appendTo(tdActions);
            var iEdit = document.createElement('i');
            $(iEdit).addClass('fa fa-edit')
                .appendTo(aEdit);
            var aDelete = document.createElement('a');
            $(aDelete).addClass('btn btn-danger col-auto m-1')
                .attr('href', "/Users/Delete/" + value.id)
                .appendTo(tdActions);
            var iDelete = document.createElement('i');
            $(iDelete).addClass('fa fa-trash')
                .appendTo(aDelete);
        });
    }
    function fillUsersPagesNav(userIndexViewModel, url, term) {
        var ul = $('#pagesNav').children('ul').first().empty();
        if (userIndexViewModel.pagesAmount > 1) {
            var _loop_1 = function (i) {
                var li = document.createElement('li');
                $(li).addClass('page-item')
                    .appendTo(ul);
                if (i === userIndexViewModel.currentPage)
                    $(li).addClass('active');
                var a = document.createElement('a');
                $(a).text(i)
                    .attr('href', '#')
                    .addClass('page-link')
                    .click(function () {
                    return __awaiter(this, void 0, void 0, function () {
                        return __generator(this, function (_a) {
                            switch (_a.label) {
                                case 0: return [4 /*yield*/, getUsers(url, i, term)];
                                case 1:
                                    _a.sent();
                                    return [2 /*return*/];
                            }
                        });
                    });
                })
                    .appendTo(li);
            };
            for (var i = 1; i <= userIndexViewModel.pagesAmount; i++) {
                _loop_1(i);
            }
        }
    }
})(Users || (Users = {}));
//# sourceMappingURL=site.js.map