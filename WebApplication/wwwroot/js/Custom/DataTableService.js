class DataTableService {
    constructor(selector) {
        this._dataTable;
        this._url = '/api/specializations/datatable';
        this._startOrderFrom = 0;
        this._selector = selector;
        this._ajaxData;
        this._columns = [];
        this._rowCallback;
        this._fnDrawCallback;
        this._createdRow;
        this._initComplete;
    }

    Destroy = () =>
        $(this._selector).DataTable().destroy();

    ForApiController(controller) {
        this._url = '/api/' + controller + '/datatable';
        return this;
    }

    AddColumn(isOrderable, isSearchable, columnName, customRender) {
        if (columnName.length != 0)//Normal column
            this._columns.push({
                data: columnName,
                searchable: isSearchable,
                orderable: isOrderable,
                render: customRender != null ? customRender :
                    ((a, b, data, d) => {
                        if (data[columnName] != null)
                            return data[columnName];
                        else
                            return "";
                    })
            });
        else //Clickable column to open details on row
            this._columns.push({
                data: null,
                className: 'details-control',
                searchable: isSearchable,
                orderable: isOrderable,
                render: customRender != null ? customRender :
                    ((a, b, data, d) => {
                        if (data[columnName] != null)
                            return data[columnName];
                        else
                            return "";
                    })
            });
        return this;
    }

    AddColumnsFromDateRange(startOn, endOn, columnName) {
        var totalDays = Math.ceil(Math.abs(new Date(startOn) - new Date(endOn)) / (1000 * 60 * 60 * 24))
        //console.log(totalDays);
        for (var i = 0; i < totalDays; i++)
            this._columns.push({
                data: columnName + i,
                searchable: false,
                orderable: false
            });
        return this;
    }

    AddColumnsFromCount(count, columnName) {
        for (var i = 0; i < count; i++)
            this._columns.push({
                data: columnName + i,
                searchable: false,
                orderable: false
            });
        return this;
    }

    StartOrderFromCol(colNumner) {
        this._startOrderFrom = colNumner;
        return this;
    }

    // (oSettings) => {}
    FnDrawCallback(expression) {
        this._fnDrawCallback = expression;
        return this;
    }

    // (row, data, dataIndex) => {}
    CreatedRow(expression) {
        this._createdRow = expression;
        return this;
    }

    // (row, data, displayNum, displayIndex, dataIndex) =>{}
    RowCallback(expression) {
        this._rowCallback = expression;
        return this;
    }

    // (settings, json) => {}
    InitComplete(expression) {
        this._initComplete = expression;
        return this;
    }

    //(data, type, row, meta) => 
    AjaxData(expression) {
        this._ajaxData = expression;
        return this;
    }


    CompleteDataTable = () => $(this._selector).DataTable({
        serverSide: true,
        responsive: true,
        processing: true,
        colReorder: true,
        lengthMenu: [[10, 25, 50, 100, 150, -1], [10, 25, 50, 100, 150, "All"]],
        iDisplayLength: 150,
        dom: 'Bfrtlp',
        //buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
        buttons: [
            {
                extend: 'excelHtml5',
                text: 'Excel',
                exportOptions: { orthogonal: 'ExcelExport' }
            }
        ],
        language: {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"></span> '
        },
        ajax: {
            url: this._url,
            type: 'POST',
            contentType: "application/json",
            data: this._ajaxData
        },
        order: [[this._startOrderFrom, 'desc']],
        columns: this._columns,
        columnDefs: [{
            targets: '_all',
            render: (data, type, row) => {
                if (type == 'ExcelExport') {
                    console.log(data);
                    return data;
                }
                else
                    return data;
            }
        }],
        fnDrawCallback: this._fnDrawCallback,
        createdRow: this._createdRow,
        rowCallback: this._rowCallback,
        initComplete: this._initComplete,
    });
}
