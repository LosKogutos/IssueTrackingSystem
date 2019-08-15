$(document).ready(function () {
    $("#Eta").datepicker({ dateFormat: 'dd M yy' });
    $("#CreatedBy_Username").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../FetchUsers',
                dataType: 'json',
                data: {
                    q: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 3,
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
});

function UpdateTicket() {
    $.ajax({
        type: 'POST',
        url: '../UpdateTicket',
        data: JSON.stringify(GetTicketViewModelObject()),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSuccess) {
                $("#successAlertDiv").show();
            }
            else {
                $("#failureAlertDiv").show();
            }
        },
        failure: function (errMsg) {
            $("#failureAlertDiv").show();
        }
    });
}

function GetTicketViewModelObject() {
    var id = $("#HiddenId").val();
    var createdBy = $("#CreatedBy_Username").val();
    var assignedTo = $("#AssignedTo_Username").val();
    var eta = $("#Eta").val();
    var description = $("#Description").val();
    var data = {
        Id: id,
        CreatedBy: {
            Username: createdBy
        },
        AssignedTo: {
            Username: assignedTo
        },
        Eta: eta,
        Description: description
    };
    return data;
}