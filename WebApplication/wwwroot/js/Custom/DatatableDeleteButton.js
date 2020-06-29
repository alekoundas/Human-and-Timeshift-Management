//Delete action in datatable
$(document).on('click', '.DatatableDeleteButton', (element) => {
    $.ajax({
        type: "DELETE",
        url: element.target.getAttribute('urlattr'),
        dataType: "json",
        success: function (response) {
            console.log('mmmkeyyyyy');
        }
    });
})