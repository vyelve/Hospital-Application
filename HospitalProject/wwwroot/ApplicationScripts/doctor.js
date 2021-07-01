var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindDoctorGrid(jsonData);
}

function BindDoctorGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }
    $('#tblDoctor').bootstrapTable('destroy');
    $('#tblDoctor').bootstrapTable({
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
            },
            {
                field: 'doctorName',
                title: 'Doctor Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'departmentName',
                title: 'Department',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'specialistName',
                title: 'Specialist',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'per_Day_Charges',
                title: 'Per Day Charges',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'doctorSchedule',
                title: 'Doctor Schedule',
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
            }]
    });
}

function getHeight() {
    return $(window).height();
}

function editRowFormatter(value, row, index) {
    return [
        '<a href="javascript:void(0);" class="editDoctor fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteDoctor(\'' + row.docID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function ReloadPage() {
    var Url = '/Doctor/Index';
    location.href = Url;
}

function DoctorModel() {
    this.doctor = {
        docID: $('#hdnDocID').val(),
        doctorId: $('#ddlDoctor option:selected').val(),
        doctorName: $('#ddlDoctor option:selected').text(),
        departmentId: $('#ddlDepartment option:selected').val(),
        departmentName: $('#ddlDepartment option:selected').text(),
        specialistID: $('#ddlSpecialist option:selected').val(),
        specialistName: $('#ddlSpecialist option:selected').text(),
        per_Day_Charges: $('#txtPerDayCharges').val(),
        doctorSchedule: $('#txtDoctorSchedule').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
    this.getDoctorModel = function () {
        return this.doctor;
    }
    this.setDoctorModel = function (Data) {
        $('#hdnDocID').val(Data.docID);
        $('#ddlDoctor').val(Data.doctorId);
        $('#ddlDepartment').val(Data.departmentId);
        $('#ddlSpecialist').val(Data.specialistID);
        $('#txtPerDayCharges').val(Data.per_Day_Charges);
        $('#txtDoctorSchedule').val(Data.doctorSchedule);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetDoctorModel = function () {
        $('#hdnDocID').val('0');
        $('#ddlDoctor option:eq(0)').attr('selected', 'selected');
        $('#ddlDoctor').prop('SelectedIndex', 0);
        $('#ddlDepartment option:eq(0)').attr('selected', 'selected');
        $('#ddlDepartment').prop('SelectedIndex', 0);
        $('#ddlSpecialist option:eq(0)').attr('selected', 'selected');
        $('#ddlSpecialist').prop('SelectedIndex', 0);
        $('#txtPerDayCharges').val('');
        $('#txtDoctorSchedule').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#doctorform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveDoctorDetails();
                window.existingModel = null;
                setTimeout(function () { ReloadPage(); }, 4000);
            }
            catch (err) {
                if (arguments !== null && arguments.callee !== null && arguments.callee.trace)
                    logError(err, arguments.callee.trace());
            }
        else {
            SetAlert('error', 'Validation failed.');
        }
    });

    $Cancelbtn.bind('click', function (e) {
        e.preventDefault();
        var _doctorModel = new DoctorModel();
        _doctorModel.resetDoctorModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

var SaveDoctorDetails = function () {
    var _doctorModel = new DoctorModel();
    var _model = _doctorModel.getDoctorModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);
    console.log(jsonData);

    $.ajax({
        url: _command === "Submit" ? '/Doctor/CreateDoctor' : '/Doctor/EditDoctor',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { doctorModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _doctorModel.resetDoctorModel();
            BindDoctorGrid(result.tblDoctor);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteDoctor(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Doctor/DeleteDoctor',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "ID": ID },
                    success: function (res) {
                        SetAlert('success', 'Record Deleted Successfully.');
                        BindDoctorGrid(res.tblDoctor);
                        setTimeout(function () { ReloadPage(); }, 4000);
                    },
                    error: function (e) {
                        SetAlert('error', 'Error Occured While Processing Data.');
                    },
                    complete: function () {
                    }
                });
            }
        }
    });
}