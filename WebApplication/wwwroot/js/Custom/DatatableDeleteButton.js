﻿//Delete action in datatable
$(document).on('click', '.DatatableDeleteButton', element =>
    swal({
        title: 'Προσοχή!',
        text: 'Είσαστε σίγουρος οτι θέλετε να διαγράψετε την οντώτητα;',
        icon: 'warning',
        closeOnClickOutside: false,
        buttons: {
            cancel: {
                text: "Απόριψη",
                value: null,
                visible: true,
                className: "",
                closeModal: true,
            },
            accept: {
                text: "Αποδοχή",
                value: true,
                visible: true,
                className: "",
                closeModal: true,
            }
        }
    }).then((value) => {
        if (value === true)
            $.ajax({
                type: "DELETE",
                url: element.target.getAttribute('urlattr'),
                dataType: "json",
                success: (response) => {
                    swal(response.responseTitle, response.responseBody, (response.isSuccessful ? 'success' : 'error'));
                }
            }).then(() =>
                [...document.getElementsByClassName("table")]
                    .forEach(table => $(table).DataTable().ajax.reload()));
    }));
