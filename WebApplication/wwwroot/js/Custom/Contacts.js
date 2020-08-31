var Count = 0;
function AppendContactForm() {

    var titleDiv = DivElement('form-group');
    var phoneNumberDiv = DivElement('form-group');
    var descriptionDiv = DivElement('form-group');
    var deleteButtonDiv = DivElement('form-group');


    titleDiv.appendChild(DivInputGroupElement('Contacts[' + Count + '].Title', 'Επαφή:', 'ContactTitleNum_' + Count));
    phoneNumberDiv.appendChild(DivInputGroupElement('Contacts[' + Count + '].PhoneNumber', 'Αριθμός', 'ContactNumberNum_' + Count));
    descriptionDiv.appendChild(DivInputGroupElement('Contacts[' + Count + '].Description', 'Περιγραφή', 'ContactDescriptionNum_' + Count));
    deleteButtonDiv.appendChild(DeleteButtonElement('DeleteButtonNum_' + Count, 'ContactDeleteButton'));

    var col8 = ColMd8Element();
    var col2 = ColMd2Element();
    var row = RowElement('ContactNum_' + Count, 'margin-top: 5%; margin-left: 5%;');


    col8.appendChild(titleDiv);
    col8.appendChild(phoneNumberDiv);
    col8.appendChild(descriptionDiv);

    col2.appendChild(deleteButtonDiv);

    row.appendChild(col8);
    row.appendChild(col2);

    document.getElementById('ContactInput').appendChild(row);
    Count++;


}

var InputElement = (name, className) => InputAttributes(document.createElement("input"), name, className);
var InputAttributes = (input, name, className) => {
    input.type = 'text';
    input.className = 'form-control ' + className;
    input.setAttribute('name', name);
    //input.setAttribute('id', name);

    return input;
};

var LablelElement = (innerText) => LabelAttributes(document.createElement("label"), innerText);
var LabelAttributes = (label, innerText) => {
    label.className = 'control-label';
    label.innerHTML = innerText;
    return label;
};

var DivInputGroupElement = (name, labelText, className) => {
    var inputGroup = DivElement('input-group');
    inputGroup.append(LablelElement(labelText));
    inputGroup.appendChild(InputElement(name, className));
    return inputGroup;
}


var DivElement = (className, style) => DivAttributes(document.createElement("div"), className, style);
var DivAttributes = (div, className, style) => {
    div.className = className;
    div.style.cssText = style;
    return div;
};

var RowElement = (className, style) => DivElement('row ' + className, style);
var ColMd8Element = () => DivElement('col-md-8');
var ColMd2Element = () => DivElement('col-md-2');


var DeleteButtonElement = (id, className) => ButtonDeleteAttributes(document.createElement("button"), id, className);
var ButtonDeleteAttributes = (button, id, className) => {
    button.type = 'button';
    button.className = 'btn btn-danger ' + className;
    button.innerHTML = '<i id= "DeleteButtonNum_' + Count + '"class="fa fa-trash"></i>';
    button.setAttribute('id', id);
    button.style.cssText = ' margin-top: 100px;';
    return button;
};







$('#ContactInput').on('click', '.ContactDeleteButton', (element) => {
    var contactToRemove = document.getElementsByClassName('ContactNum_' + element.target.id.split('_')[1]);
    if (contactToRemove.length > 0)
        contactToRemove[0].remove();
})








