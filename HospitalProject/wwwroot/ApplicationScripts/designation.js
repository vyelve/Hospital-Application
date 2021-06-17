var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindDesignationGrid(jsonData);
}

function BindDesignationGrid(jsonData) {

    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblDesignation').bootstrapTable('destroy');
    $('#tblDesignation').bootstrapTable({
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
                field: 'designationName',
                title: 'Designation',
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
        '<a href="javascript:void(0);" class="editdesignation fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteDesignation(\'' + row.designationID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function DesignationModel() {
    this.designation = {
        designationID: $('#hdnDesignationID').val(),
        designationName: $('#txtDesignationName').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
    this.getDesignationModel = function () {
        return this.designation;
    }
    this.setDesignationModel = function (Data) {
        $('#hdnDesignationID').val(Data.designationID);
        $('#txtDesignationName').val(Data.designationName);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetDesignationModel = function () {
        $("#hdnDesignationID").val('0');
        $('#txtDesignationName').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#designationform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveDesignationData();
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
        var _designationModel = new DesignationModel();
        _designationModel.resetDesignationModel();
        Window.existingModel = null;

        var Url = '/Designation/Index';
        location.href = Url;
    });
});

var SaveDesignationData = function () {

    var _designationModel = new DesignationModel();
    var _model = _designationModel.getDesignationModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/Designation/CreateDesignation' : '/Designation/EditDesignation',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { designationModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been added.');
            }
            else {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been updated.');
            }
            _designationModel.resetDesignationModel();
            BindDesignationGrid(result.tblDesignation);
        },
        error: function (e) {
            SetAlertDiv("myAlert", true, 'Warning! ', ' Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}   

function DeleteDesignation(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Designation/DeleteDesignation',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "DesignationId": ID },
                    success: function (res) {
                        SetAlertDiv("myAlert", false, 'Success! ', ' Record Deleted Successfully');
                        BindDesignationGrid(res.tblDesignation);
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
