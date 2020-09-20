$(() => {
    $('#UploadExcel').on('click', () => {
        var fdata = new FormData();
        var fileExtension = ['xls', 'xlsx'];
        //var filename = $('#ImportExcel').val();
        var input = document.getElementById('ImportExcel');

        var fileUpload = $('#ImportExcel').get(0);
        var files = fileUpload.files;

        if (input.value.length == 0) {
            alert("Παρακαλώ επέλεξε αρχείο.");
            return false;
        }
        else {
            var extension = input.value.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                alert("Μονο αρχεια excel ειναι επιτρεπτα.");
                return false;
            }
        }
        fdata.append(files[0].name, files[0]);

        $.ajax({
            type: 'POST',
            url: '/' + document.getElementById('ImportExcel').dataset.AjaxUrl+'/Import',
            beforeSend: (xhr) => {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            success: (response) => {
                if (response.length == 0)
                    alert('Some error occured while uploading');
                else {
                    $('#divPrint').html(response);
                }
            },
            error: (e) =>{
                $('#divPrint').html(e.responseText);
            }
        });
    })
});