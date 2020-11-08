//Delete action in datatable
$(document).on('click', '.DatatableDeactivateButton', element =>
    swal({
        title: 'Προσοχή!',
        text: 'Είσαστε σίγουρος οτι θέλετε να αντιστέψετε την κατάσταση της οντώτητα;',
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
                type: "GET",
                url: element.target.getAttribute('urlattr'),
                dataType: "json",
                success: (response) => {
                    console.log(response);
                    swal(response.responseTitle, response.responseBody, (response.isSuccessful ? 'success' : 'error'));
                }
            }).then(() =>
                [...document.getElementsByClassName("table")]
                    .forEach(table => $(table).DataTable().ajax.reload()));
    }));
