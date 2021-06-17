var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindNurseGrid(jsonData);
}

function BindNurseGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }
    $('#tblNurse').bootstrapTable('destroy');
    $('#tblNurse').bootstrapTable({
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
                field: 'nurseName',
                title: 'Nurse Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'nurseType',
                title: 'Type',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'shiftType',
                title: 'Shift Type',
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
        '<a href="javascript:void(0);" class="editNurse fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteNurse(\'' + row.nurID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function reloadPage() {
    var Url = '/Nurse/Index';
    location.href = Url;
}

function NurseModel() {
    this.nurse = {
        nurID: $('#hdnNurID').val(),
        nurseId: $('#ddlNurse option:selected').val(),
        nurseName: $('#ddlNurse option:selected').text(),
        nurseType: $('#ddlNurseType option:selected').val(),
        shiftType: $('#ddlShiftType option:selected').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
        this.getNurseModel = function () {
            return this.nurse;
        }
    this.setNurseModel = function (Data) {
        $('#hdnNurID').val(Data.nurID);
        $('#ddlNurse').val(Data.nurseId);
        $('#ddlNurseType').val(Data.nurseType.trim());
        $("#ddlNurseType option:contains(" + Data.nurseType.trim() + ")").attr('selected', true);
        $('#ddlShiftType ').val(Data.shiftType.trim());
        $("#ddlShiftType option:contains(" + Data.shiftType.trim() + ")").attr('selected', true);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetNurseModel = function () {
        $('#hdnNurID').val('0');
        $('#ddlNurse option:eq(0)').attr('selected', 'selected');
        $('#ddlNurse').prop('SelectedIndex', 0);
        $('#ddlNurse').val('');
        $('#ddlNurseType').val('');
        $('#ddlNurseType option:selected').removeAttr('selected');

        $('#ddlShiftType').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {

    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#nurseform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveNurseDetails();
                window.existingModel = null;
                reloadPage();
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
        var _nurseModel = new NurseModel();
        _nurseModel.resetNurseModel();
        Window.existingModel = null;
        reloadPage();
    });
});

var SaveNurseDetails = function () {
    var _nurseModel = new NurseModel();
    var _model = _nurseModel.getNurseModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);
    console.log(jsonData);

    $.ajax({
        url: _command === "Submit" ? '/Nurse/CreateNurse' : '/Nurse/EditNurse',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { nurseModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been added.');
            }
            else {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been updated.');
            }
            _nurseModel.resetNurseModel();
            console.log(result.tblNurse);
            BindNurseGrid(result.tblNurses);
        },
        error: function (e) {
            SetAlertDiv("myAlert", true, 'Warning! ', ' Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteNurse(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Nurse/DeleteNurse',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "nurseId": ID },
                    success: function (res) {
                        SetAlertDiv("myAlert", false, 'Success! ', ' Record Deleted Successfully');
                        BindNurseGrid(res.tblNurses);
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