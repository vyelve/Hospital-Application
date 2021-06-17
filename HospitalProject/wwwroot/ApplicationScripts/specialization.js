var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindSpecializationGrid(jsonData);
}


function BindSpecializationGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblSpecialization').bootstrapTable('destroy');
    $('#tblSpecialization').bootstrapTable({
        data: jsonData,
        height: 500,
        pagination: true,
        pageSize: 10,
        pageList: [10, 25, 50, 100, 200],
        search: true,
        showColumns: false,
        showRefresh: false,
        cache: false,
        striped: false,
        showExport: true,
        exportTypes: ['json', 'xml', 'csv', 'txt', 'sql', 'excel', 'pdf'],
        columns: [
            {
                field: 'specializationName',
                title: 'Specialization',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'specializationDescription',
                title: 'Specialization Description',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'isActive',
                title: 'Active',
                align: 'center',
                valign: 'middle',
                sortable: true,
                width: '5%'
            },
            {
                field: 'Edit',
                title: 'Edit',
                align: 'Center',
                valign: 'bottom',
                sortable: false,
                editable: false,
                formatter: editRowFormatter,
                events: window.operateEvents,
                width: '5%'
            },
            {
                field: 'Delete',
                title: 'Delete',
                align: 'Center',
                valign: 'bottom',
                sortable: false,
                editable: false,
                formatter: deleteRowFormatter,
                width: '5%'
            }]
    });
}

function getHeight() {
    return $(window).height();
}

function editRowFormatter(value, row, index) {
    return [
        '<a href="javascript:void(0);" class="editspecialization fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteSpecialization(\'' + row.specialistID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function SpecializationModel() {
    this.specialization = {
        specialistID: $('#hdnSpecialistID').val(),
        specializationName: $('#txtSpecializationName').val(),
        specializationDescription: $('#txtSpecializationDescription').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
    this.getSpecializationModel = function () {
        return this.specialization;
    }
    this.setSpecializationModel = function (Data) {
        $('#hdnSpecialistID').val(Data.specialistID);
        $('#txtSpecializationName').val(Data.specializationName);
        $('#txtSpecializationDescription').val(Data.specializationDescription);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetSpecializationModel = function () {
        $("#hdnSpecialistID").val('0');
        $('#txtSpecializationName').val('');
        $('#txtSpecializationDescription').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#specializationform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveSpecializationData();
                window.existingModel = null;
            }
            catch (err) {
                if (arguments !== null && arguments.callee !== null && arguments.callee.trace)
                    logError(err, arguments.callee.trace());
            }
        else {
            SetAlertDiv("myAlert", true, 'Warning! ', 'Validation failed.');
        }
    });

    $Cancelbtn.bind('click', function (e) {
        e.preventDefault();
        var _specializationModel = new SpecializationModel();
        _specializationModel.resetSpecializationModel();
        Window.existingModel = null;

        var Url = '/Specialization/Index';
        location.href = Url;
    });
});

var SaveSpecializationData = function () {
    var _specializationModel = new SpecializationModel();
    var _model = _specializationModel.getSpecializationModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/Specialization/CreateSpecialization' : '/Specialization/EditSpecialization',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { specializationModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been added.');
            }
            else {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been updated.');
            }
            _specializationModel.resetSpecializationModel();
            BindSpecializationGrid(result.tblSpecialization);
        },
        error: function (e) {
            SetAlertDiv("myAlert", true, 'Warning! ', ' Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}
function DeleteSpecialization(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Specialization/DeleteSpecialization',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "specialistID": ID },
                    success: function (res) {
                        SetAlertDiv("myAlert", false, 'Success! ', ' Record Deleted Successfully');
                        BindSpecializationGrid(res.tblSpecialization);
                    },
                    error: function (e) {
                        SetAlertDiv("myAlert", true, 'Warning! ', 'Error Occured While Processing Data.');
                    },
                    complete: function () {
                    }
                });
            }
        }
    });
}