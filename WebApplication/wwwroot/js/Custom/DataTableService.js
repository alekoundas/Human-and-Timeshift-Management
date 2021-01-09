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
        this._footerCallback;
        this._fixedLeftColums = 0;
        this._fixedRightColums = 0;
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


    FixedLeftColums(leftColumns) {
        this._fixedLeftColums = leftColumns;
        return this;
    };

    FixedRightColums(rightColumns) {
        this._fixedRightColums = rightColumns;
        return this;
    };

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
    // (row, data, start, end, display) =>{}
    FooterCallback(expression) {
        this._footerCallback = expression;
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

    HideEmptyColumns() {
        this._initComplete = () => HideEmptyColumnss(this);
        return this;
    }

    CompleteDataTable = () => {
        return $(this._selector).DataTable({
            serverSide: true,
            responsive: true,
            processing: true,
            autoWidth: false,
            colReorder: true,
            scrollX: true,
            fixedColumns: {
                leftColumns: this._fixedLeftColums,
                rightColumns: this._fixedRightColums
            },
            lengthMenu: [[10, 25, 50, 100, 150, -1], [10, 25, 50, 100, 150, "All"]],
            iDisplayLength: 150,
            dom: 'Bfrtlp',
            buttons: [/*'copy', 'csv', */'excel', /*'pdf',*/ 'print'],
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
                render: (data, type, row) => data
            }],
            fnDrawCallback: this._fnDrawCallback,
            createdRow: this._createdRow,
            rowCallback: this._rowCallback,
            initComplete: this._initComplete,
            footerCallback: this._footerCallback
        });

    }





    //https://datatables.net/forums/discussion/51536/hide-empty-columns-in-responsive-datatables
    //HideEmptyColumnss = (selector) => {
    //    var emptyColumnsIndexes = []; // store index of empty columns here
    //    // check each column separately for empty cells
    //    $(selector).find('th').each(function (i) {
    //        // get all cells for current column
    //        var cells = $(this).parents('table').find('tr td:nth-child(' + (i + 1) + ')');
    //        var emptyCells = 0;

    //        cells.each(function (cell) {
    //            // increase emptyCells if current cell is empty, trim string to remove possible spaces in cell
    //            if ($(this).html().trim() === '') {
    //                emptyCells++;
    //            }
    //        });

    //        // if all cells are empty push current column to emptyColumns
    //        if (emptyCells === $(cells).length) {
    //            emptyColumnsIndexes.push($(this).index());
    //        }
    //    });

    //    // only make changes if there are columns to hide
    //    if (emptyColumnsIndexes.length > 0) {
    //        /* add class never to all empty columns
    //            never is a special class of the Responsive extension:
    //            Columns with class never will never be visible, regardless of the browser width, and the data will not be shown in a child row
    //        */
    //        $((selector).DataTable().columns(emptyColumnsIndexes).header()).addClass('never');
    //        // Recalculate the column breakpoints based on the class information of the column header cells, class never will now be available to Responsive extension
    //        $(selector).DataTable().columns.adjust().responsive.rebuild();
    //        // immediatly call recalc to have Responsive extension updae the display for the cahnge in classes
    //        $(selector).DataTable().columns.adjust().responsive.recalc();
    //    }
    //}
    //}
}
