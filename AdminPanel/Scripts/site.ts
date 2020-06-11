function getUrl(url: string) {
    const path = window.location.pathname;

    if (path != '' && path != '/' && path != null) {
        return `${path}/${url}`;
    }

    return url;
}

namespace Common {
    export class ItemViewModel {
        id: string;
        name: string;
    }

    function setAddButtonAvailability(select: HTMLElement, addButton: HTMLElement): void {
        if ($(select).children().length > 0) {
            $(addButton).removeClass('disabled')
                .prop('disabled', false);
        }
        else {
            $(addButton).addClass('disabled')
                .prop('disabled', true);
        }
    }

    function removeOption(select: HTMLElement, addButton: HTMLElement, value: string | number | string[]) {
        $(select).children('option').each(function (index, elem) {
            if ($(elem).val() == value) {
                $(elem).remove();
            }
        });

        setAddButtonAvailability(select, addButton);
    }

    function addOption(select: HTMLElement, addButton: HTMLElement, text: string, value: string) {
        const option = document.createElement('option');
        $(option).val(value)
            .text(text)
            .appendTo(select);

        setAddButtonAvailability(select, addButton);
    }

    function removeRow(select: HTMLElement, table: HTMLElement, addButton: HTMLElement, deleteButton: HTMLElement): void {
        let text;
        var value;

        const tr = $(deleteButton).closest('tr');

        const idInput = $(tr).find(':input:hidden').first();
        value = idInput.val();
        text = idInput.closest('td').text();

        tr.remove();

        addOption(select, addButton, text, value);
    }

    function addRow(select: HTMLSelectElement, table: HTMLElement, addButton: HTMLElement): void {
        const selectedIndex = $(select).prop('selectedIndex');
        const selectedItem = $(select).children().eq(selectedIndex);
        const value = $(selectedItem).val();
        const label = $(selectedItem).text();

        const tbody = $(table).find('tbody');

        const tableId = $(table).attr('id');

        const row = document.createElement('tr');
        $(row).appendTo(tbody);

        const col1 = document.createElement('td');
        $(col1).text(label)
            .appendTo(row);

        const inputName = document.createElement('input');
        $(inputName).val(value)
            .attr('type', 'hidden')
            .attr('name', `${tableId}`)
            .appendTo(col1);

        const col2 = document.createElement('td');
        $(col2).appendTo(row);

        const deleteBtn = document.createElement('button');
        $(deleteBtn).attr('type', 'button')
            .addClass('btn btn-danger')
            .click(function (event) {
                removeRow(select, table, addButton, event.currentTarget);
            })
            .appendTo(col2);

        const deleteI = document.createElement('i');
        $(deleteI).addClass('fa fa-trash')
            .appendTo(deleteBtn);

        removeOption(select, addButton, value);
    }

    export function connectSelectToTable(select: HTMLSelectElement, addButton: HTMLElement, table: HTMLTableElement) {

        setAddButtonAvailability(select, addButton);

        $(addButton).click(function () {
            addRow(select, table, addButton);
        });

        $(table).find('.fa-trash').each(function (index, elem) {
            $(elem).closest('button').click(() =>
                removeRow(select, table, addButton, elem));
        });
    }
}

namespace InfoBases {
    export class InfoBaseItemViewModel extends Common.ItemViewModel {}

    export class InfoBaseViewModel {
        id: string;
        name: string;
        server: string;
        infoBaseName: string;
        iBasesContent: string;
        infoBasesLists: InfoBasesLists.InfoBasesListItemViewModel[];
    }

    export class InfoBaseIndexViewModel {
        currentPage: number;
        pagesAmount: number;
        items: InfoBaseViewModel[];
    }

    function showFormGroupFor(elem): void {
        $(elem).closest('.form-group')
            .show();
    }

    function hideFormGroupFor(elem): void {
        $(elem).val('')
            .closest('.form-group')
            .hide();
    }

    function setGroupsVisibility() {
        const select = $('#ConnectionType');
        const selectedIndex = $(select).prop('selectedIndex');
        const value = $(select).children().eq(selectedIndex).val();

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

    export function initInfoBaseCreateEditView(): void {

        setGroupsVisibility();

        $('#ConnectionType').change(function(e, t) {
            setGroupsVisibility();
        });
    }
}

namespace InfoBasesLists {
    export class InfoBasesListItemViewModel extends Common.ItemViewModel {}

    export class InfoBasesListViewModel{
        id: string;
        name: string;
        listId: string;
        infoBases: InfoBases.InfoBaseItemViewModel[];
        users: Common.ItemViewModel[];
    }

    export class InfoBasesListIndexViewModel {
        currentPage: number;
        pagesAmount: number;
        items: InfoBasesListViewModel[];
    }
}

namespace Users {
    export class UserViewModel {
        id: string;
        name: string;
        sid: string;
        samAccountName: string;
        infoBasesListId: string;
        infoBasesList: Common.ItemViewModel;
    }

    export class UserIndexViewModel {
        currentPage: number;
        pagesAmount: number;
        items: UserViewModel[];
    }

    export async function updateFromActiveDirectory(): Promise<void> {
        const btn = $('#updateFromAd');

        btn.addClass('disabled')
            .prop('disabled', true);

        const response = await fetch('/Users/UpdateFromActiveDirectory');

        if (response.ok) {
            location.reload();
        } else {
            alert('Не удалось обновить список пользователей');
        }

        btn.removeClass('disabled')
            .prop('disabled', false);
    }

    export async function initIndexView(): Promise<void> {
        const url = getUrl('Users/GetUsers');

        await getUsers(url, 1, '');

        var time = 0;

        $('#searchBox').keyup(function (event) {
            clearTimeout(time);

            time = window.setTimeout(async function () {
                const term = $('#searchBox').val();
                await getUsers(url, 1, term);
            }, 500);
        });
    }

    async function getUsers(url: string, pageIndex: number, term: string | number | string[]): Promise<void> {
        let urlParams = `${url}?pageIndex=${pageIndex}`

        if (term != '')
            urlParams += `&term=${term}`;

        const response = await fetch(urlParams);

        if (response.ok) {
            const data: Users.UserIndexViewModel = await response.json();

            fillUsersTable(data);

            fillUsersPagesNav(data, url, term);
        } else {
            alert('Не удалось получить список пользователей');
        }
    }

    function fillUsersTable(userIndexViewModel: Users.UserIndexViewModel): void {
        const tbody = $('#dataTable').children('tbody').first().empty();

        userIndexViewModel.items.forEach(function (value) {
            const tr = document.createElement('tr');
            $(tr).appendTo(tbody);

            const tdName = document.createElement('td');
            $(tdName).text(value.name)
                .appendTo(tr);

            const tdSamAccountName = document.createElement('td');
            $(tdSamAccountName).text(value.samAccountName)
                .appendTo(tr);

            const tdInfoBasesListName = document.createElement('td');
            $(tdInfoBasesListName).appendTo(tr);

            if (value.infoBasesList != null)
                $(tdInfoBasesListName).text(value.infoBasesList.name)

            const tdActions = document.createElement('td');
            $(tdActions).appendTo(tr);

            const aEdit = document.createElement('a');
            $(aEdit).addClass('btn btn-secondary col-auto m-1')
                .attr('href', `/Users/Edit/${value.id}`)
                .appendTo(tdActions);

            const iEdit = document.createElement('i');
            $(iEdit).addClass('fa fa-edit')
                .appendTo(aEdit);

            const aDelete = document.createElement('a');
            $(aDelete).addClass('btn btn-danger col-auto m-1')
                .attr('href', `/Users/Delete/${value.id}`)
                .appendTo(tdActions);

            const iDelete = document.createElement('i');
            $(iDelete).addClass('fa fa-trash')
                .appendTo(aDelete);
        });
    }

    function fillUsersPagesNav(userIndexViewModel: Users.UserIndexViewModel, url: string, term: string | number | string[]): void {
        const ul = $('#pagesNav').children('ul').first().empty();

        if (userIndexViewModel.pagesAmount > 1) {
            for (let i = 1; i <= userIndexViewModel.pagesAmount; i++) {
                const li = document.createElement('li');
                $(li).addClass('page-item')
                    .appendTo(ul);

                if (i === userIndexViewModel.currentPage)
                    $(li).addClass('active');

                const a = document.createElement('a');
                $(a).text(i)
                    .attr('href', '#')
                    .addClass('page-link')
                    .click(async function () {
                        await getUsers(url, i, term);
                    })
                    .appendTo(li);
            }
        }
    }
}