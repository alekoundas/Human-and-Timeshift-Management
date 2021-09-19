class AppendListService {
    constructor(selector) {
        this._selector = selector;
        this._url;
        this._controller;
        this._title = '';
        this._entityName = '';
        this._existingIds = [];
        this._isEdit = false;
        this._select2Count = 0;

        this._onAfterDelete = () => { };
        this._onAfterSelect2Change = () => { };
    }

    Init = () => ({
        Listener: {
            OnAfterDelete: (callback) => {
                this._onAfterDelete = callback;
                return this.Init();
            },
            OnAfterSelect2Change: (callback) => {
                this._onAfterSelect2Change = callback;
                return this.Init();
            }
        },
        ForApiController: (controller) => {
            this._controller = controller;
            return this.Init();
        },
        SetTitle: (title) => {
            this._title = title;
            return this.Init();
        },
        SetEditState: (isEdit) => {
            this._isEdit = isEdit;
            return this.Init();
        },
        AppendExistingIds: (ids) => {
            this._existingIds = ids;
            return this.Init();
        },
        SetEntityName: (entityName) => {
            this._entityName = entityName;
            return this.Init();
        },
        Complete: () => {
            //Add title with an 'add' button and an event listener
            document.getElementById(this._selector).innerHTML = this.#GenerateHTMLMain();

            if (this._isEdit === true)
                document.getElementById("AddSelect2Btn").addEventListener("click", () => this.#AppendNewSelect2());

            //Add existing select2 if any
            if (this._existingIds != undefined)
                [...this._existingIds].forEach(x => this.#AppendNewSelect2(x))

            //return this.#OnAfterDelete()
        }
    })

    Set = {
        ErrorOnEntityId: (id, text) => {
            var num = this.#GetSelectedEntities().filter(x => x.id == id)[0].num;
            document.getElementById('Select2ErrorSpanNum_' + num).innerHTML =
                document.getElementById('Select2ErrorSpanNum_' + num).innerHTML + text + '<br>';

            document.getElementById('Select2ErrorSpanNum_' + num).style.display = '';
            return this.Set;
        },
        WarningOnEntityId: (id, text) => {
            var num = this.#GetSelectedEntities().filter(x => x.id == id)[0].num;
            document.getElementById('Select2WarningSpanNum_' + num).innerHTML =
                document.getElementById('Select2WarningSpanNum_' + num).innerHTML + text + '<br>';
            document.getElementById('Select2WarningSpanNum_' + num).style.display = '';
            return this.Set;
        },
        ClearErrorOnEntityId: (id) => {
            var num = this.#GetSelectedEntities().filter(x => x.id == id)[0].num;
            document.getElementById('Select2ErrorSpanNum_' + num).innerHTML = '';
            document.getElementById('Select2ErrorSpanNum_' + num).style.display = 'none';
            return this.Set;
        },
        ClearWarningOnEntityId: (id) => {
            var num = this.#GetSelectedEntities().filter(x => x.id == id)[0].num;
            document.getElementById('Select2WarningSpanNum_' + num).innerHTML = '';
            document.getElementById('Select2WarningSpanNum_' + num).style.display = 'none';
            return this.Set;
        },
        ClearAllError: () => {
            this.#GetSelectedEntities().map(x => x.num).forEach(x => {
                document.getElementById('Select2ErrorSpanNum_' + x).innerHTML = '';
                document.getElementById('Select2ErrorSpanNum_' + x).style.display = 'none';
            });
            return this.Set;
        },
        ClearAllWarning: () => {
            this.#GetSelectedEntities().map(x => x.num).forEach(x => {
                document.getElementById('Select2WarningSpanNum_' + x).innerHTML = '';
                document.getElementById('Select2WarningSpanNum_' + x).style.display = 'none';
            });
            return this.Set;
        }

    }

    Retrieve = {

        SelectedIds: () => {
            return this.#GetSelectedEntities().map(x => x.id);
        },
        EntityErrors: () => {
            return this.#GetSelectedEntities().filter(x => x.error.
                replace(/\s/g, "") != '')//Remove whitespace
                .map(x => ({ id: x.id, error: x.error }));
        }
    }



    //Append select2 with buttons and event listeners
    #AppendNewSelect2(id, text) {
        document.getElementById('Select2Container').insertAdjacentHTML('beforeend', this.#GenerateHTMLSelect2(this._select2Count, id));
        this.#Select2Init(this._select2Count, id, text);

        if (this._isEdit === true)
            document.getElementById('Select2DeleteButtonNum_' + this._select2Count).addEventListener('click', (e) => this.#DeleteSelect2(e));

        this._select2Count++;
    };

    //Remove entity and re-append enerything from start
    #DeleteSelect2(e) {
        var num = e.target.id.split('_')[1];
        document.getElementById('Select2DivNum_' + num).remove()

        this.#RefreshRows();
        this._onAfterDelete(this);
    };

    //Select2 - Library
    #Select2Init = (select2Count, existingId, existingText) => {

        //Get data for pre set ids (Select2 loads data on 'open' only)
        $('#Select2EntityNum_' + select2Count).select2({
            ajax: {
                type: "POST",
                url: '/api/' + this._controller + '/select2',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: params => JSON.stringify(
                    new DtoFactory().Select2(
                        params.search,
                        params.page,
                        0,
                        this.#GetSelectedEntities().map(x => x.id)))
            }
        })

        if (existingId > 0 || existingId?.length == 36) {
            if (existingText == undefined) {
                $.ajax({
                    type: "POST",
                    url: '/api/' + this._controller + '/select2',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(new DtoFactory().Select2(null, null, existingId))
                }).then((data) => {
                    $('#Select2EntityNum_' + select2Count).data('select2').trigger('select', {
                        data: { id: existingId, text: data.results[0].text }
                    });
                });
            }
            else {
                $('#Select2EntityNum_' + select2Count).data('select2').trigger('select', {
                    data: { id: existingId, text: existingText }
                });
            }
        }


        $('#Select2EntityNum_' + select2Count).select2({
            ajax: {
                type: "POST",
                url: '/api/' + this._controller + '/select2',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: params => JSON.stringify(
                    new DtoFactory().Select2(
                        params.search,
                        params.page,
                        0,
                        this.#GetSelectedEntities().map(x => x.id)))
            }
        }).on('change', (e) => {
            //this.#RefreshRows();
            this._onAfterSelect2Change(e.target.value)
        });

        if (this._isEdit === false)
            $('#Select2EntityNum_' + select2Count).prop('disabled', true);
    }


    #RefreshRows() {
        this._select2Count = 0;
        var entities = this.#GetSelectedEntities();
        document.getElementById('Select2Container').innerHTML = '';
        entities.forEach(data => this.#AppendNewSelect2(data.id, data.text));
    }


    #GetSelectedEntities() {
        return [...document.getElementsByClassName('Select2Entity')]
            .map(select2 => ({
                id: select2.value,
                text: select2.outerText,
                num: select2.id.split('_')[1],
                error: document.getElementById('Select2ErrorSpanNum_' + select2.id.split('_')[1]).innerHTML
            }))
            .filter(data => data.id != "");
    };


    #GenerateHTMLMain = () =>
        '<div style="width:100%; text-allign:center;">' +
        this._title +
        (this._isEdit === true ? '<button id="AddSelect2Btn" type="button" class="btn btn-success"><i class="fa fa-plus"></i></button>' : '') +
        '</div>' +
        '<div style="width:100%;" id="Select2Container">' +
        '</div>';

    #GenerateHTMLSelect2 = (count) =>
        '<div style="width:100%; padding-top:10px;"class="Select2Div"  id="Select2DivNum_' + count + '">' +
        '<div style="width: 70%; float: left;">' +
        '<select id="Select2EntityNum_' + count + '"class="Select2Entity" style="width:100%" name="' + this._entityName + '[' + count + ']"></select >' +
        '<span style="display:none; color:red;" id="Select2ErrorSpanNum_' + count + '" class="Select2ErrorSpan">     </span>' +
        '<span style="display:none; color:orange;" id="Select2WarningSpanNum_' + count + '" class="Select2WarningSpan">   </span>' +
        '</div >' +
        '<div style="width: 5%; float: left;">&nbsp;</div >' +
        '<div style="width: 20%; float: left;">' +
        (this._isEdit === true ? '<button class= "btn btn-danger Select2DeleteButton" type="button" id="Select2DeleteButtonNum_' + count + '"> <i class="fa fa-trash Select2DeleteButton" id="Select2DeleteFafaNum_' + count + '"></i></button >' : '') +
        '</div >' +
        '</div >';
}
