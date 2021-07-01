var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
var _roomCharges = 0;
var _doctorCharges = 0;

window.existingModel = null;

if (jsonData !== null) {
    BindPatientBillDetailsGrid(jsonData);
}

function BindPatientBillDetailsGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isDischarge = jsonData[i].isDischarge === true ? "Yes" : "No";
    }
    $('#tblPatientBillDetails').bootstrapTable('destroy');
    $('#tblPatientBillDetails').bootstrapTable({
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
                field: 'billNumber',
                title: 'Bill Number',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'patientName',
                title: 'Patient Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
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
                field: 'roomName',
                title: 'Room Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'medicineBill',
                title: 'Medicine Bill',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'totalBill',
                title: 'Total Bill',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'paidBill',
                title: 'Paid Bill',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'remainingBill',
                title: 'Remaining Bill',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'isDischarge',
                title: 'Discharge',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            }]
    });
}

function getHeight() {
    return $(window).height();
}

function editRowFormatter(value, row, index) {
    if (row.isDischarge.toLowerCase() == "yes") {
        return [
            '<a href="javascript:void(0);" class="fas fa-exclamation-circle" style="pointer-events: none;" data-title="Edit" title="Edit"></a>'
        ].join('');
    } else {
        return [
            '<a href="javascript:void(0);" class="editPatientBillDetails fas fa-edit" data-title="Edit" title="Edit"></a>'
        ].join('');
    }
}

function ReloadPage() {
    var Url = '/PatientBillDetail/Index';
    location.href = Url;
}

function PatientBillDetailModel() {
    this.patientBill = {
        billId: $('#hdnBillId').val(),
        billNumber: $('#hdnBillNumber').val(),
        patientId: $('#ddlPatient option:selected').val(),
        patientName: $('#ddlPatient option:selected').text(),
        roomId: $('#ddlRoom option:selected').val(),
        roomName: $('#ddlRoom option:selected').text(),
        no_of_Days_Admitted: $('#txtNoOfDaysAdmitted').val(),
        roomCharges: $('#txtRoomCharges').val(),
        doctorId: $('#ddlDoctor option:selected').val(),
        doctorName: $('#ddlDoctor option:selected').text(),
        doctorCharges: $('#txtDoctorCharges').val() == '' ? 0 : $('#txtDoctorCharges').val(),
        medicineBill: $('#txtMedicineBill').val() == '' ? 0 : $('#txtMedicineBill').val(),
        totalBill: $('#txtTotalBill').val() == '' ? 0 : $('#txtTotalBill').val(),
        paidBill: $('#txtPaidBill').val() == '' ? 0 : $('#txtPaidBill').val(),
        remainingBill: $('#txtRemainingBill').val() == '' ? 0 : $('#txtRemainingBill').val(),
        isDischarge: $('#chkIsDischarge').prop('checked'),
    },
        this.getPatientBillDetailModel = function () {
            return this.patientBill;
        }
    this.setPatientBillDetailModel = function (Data) {
        $('#hdnBillId').val(Data.billId);
        $('#hdnBillNumber').val(Data.billNumber);
        $('#ddlPatient').val(Data.patientId);
        $('#ddlRoom').val(Data.roomId);
        $('#txtNoOfDaysAdmitted').val(Data.no_of_Days_Admitted);
        $('#txtRoomCharges').val(Data.roomCharges);
        $('#ddlDoctor').val(Data.doctorId);
        $('#txtDoctorCharges').val(Data.doctorCharges);
        $('#txtMedicineBill').val(Data.medicineBill);
        $('#txtTotalBill').val(Data.totalBill);
        $('#txtPaidBill').val(Data.paidBill);
        $('#txtRemainingBill').val(Data.remainingBill);
        Data.isDischarge === "Yes" ? $('#chkIsDischarge').prop('checked', true).change() : $('#chkIsDischarge').prop('checked', false).change();
        $("#btnSubmit").val('Update');
        _roomCharges = Data.roomCharges;
        _doctorCharges = Data.doctorCharges;
        if (Data.isDischarge == "Yes") {
            $('#ddlPatient').prop('disabled', true);
            $('#ddlRoom').prop('disabled', true);
            $('#ddlDoctor').prop('disabled', true);
            $('#txtNoOfDaysAdmitted').prop('disabled', true);
            $('#txtMedicineBill').prop('disabled', true);
            $("#btnSubmit").hide();
        }
    }
    this.resetPatientBillDetailModel = function () {
        $('#hdnBillId').val('0');
        $('#hdnBillNumber').val('');
        $('#ddlPatient option:eq(0)').attr('selected', 'selected');
        $('#ddlPatient').prop('SelectedIndex', 0);
        $('#ddlRoom option:eq(0)').attr('selected', 'selected');
        $('#ddlRoom').prop('SelectedIndex', 0);
        $('#txtNoOfDaysAdmitted').val('');
        $('#txtRoomCharges').val('');
        $('#ddlDoctor option:eq(0)').attr('selected', 'selected');
        $('#ddlDoctor').prop('SelectedIndex', 0);
        $('#txtDoctorCharges').val('');
        $('#txtMedicineBill').val('');
        $('#txtTotalBill').val('');
        $('#txtPaidBill').val('');
        $('#txtRemainingBill').val('');
        $('#chkIsDischarge').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#patientBillform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SavePatientBillDetails();
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
        var _patientBillDetailModel = new PatientBillDetailModel();
        _patientBillDetailModel.resetPatientBillDetailModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

var SavePatientBillDetails = function () {
    var _patientBillDetailModel = new PatientBillDetailModel();
    var _model = _patientBillDetailModel.getPatientBillDetailModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);
    console.log(jsonData);

    $.ajax({
        url: _command === "Submit" ? '/PatientBillDetail/CreatePatientBillDetail' : '/PatientBillDetail/EditPatientBillDetail',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { patientBillModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _patientBillDetailModel.resetPatientBillDetailModel();
            console.log(result.tblPatientBillDetails);
            BindPatientBillDetailsGrid(result.tblPatientBillDetails);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

$('#ddlRoom').change(function () {
    _roomCharges = GetPerDayCharges('room', this.value);
    $('#txtNoOfDaysAdmitted').focus();
});

$('#ddlDoctor').change(function () {
    _doctorCharges = GetPerDayCharges('doctor', this.value);
    $('#txtNoOfDaysAdmitted').focus();
});

function GetPerDayCharges(chargesOf, ID) {
    var perDayCharges = 0;
    if (ID != '') {
        $.ajax({
            url: chargesOf === "room" ? '/PatientBillDetail/GetRoomCharges' : '/PatientBillDetail/GetDoctorCharges',
            dataType: "JSON",
            type: "GET",
            cache: false,
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: { ID: ID },
            async: false,
            success: function (result) {
                if (result != '') {
                    perDayCharges = result;
                }
            },
            error: function (e) {
                SetAlert('error', 'Error Occured While Processing Data.');
            },
            complete: function () {
            }
        });
    }
    return perDayCharges;
}

$("#txtNoOfDaysAdmitted").on("keyup change focusin", function () {
    $('#txtRoomCharges').val('');
    $('#txtDoctorCharges').val('');
    $('#txtTotalBill').val('');
    var Rcharges = 0;
    var DCharges = 0;

    if (this.value != '') {
        Rcharges = parseInt(this.value) * _roomCharges;
        DCharges = parseInt(this.value) * _doctorCharges;
        $('#txtRoomCharges').val(Rcharges);
        $('#txtDoctorCharges').val(DCharges);
        $('#txtTotalBill').val(Rcharges + DCharges);
    }
});

$('#txtMedicineBill').on('keyup change', function () {
    $('#txtTotalBill').val('');
    if (this.value != '') {
        var _Rcharges = parseInt($('#txtRoomCharges').val());
        var _DCharges = parseInt($('#txtDoctorCharges').val());
        var _total = parseInt(this.value) + _Rcharges + _DCharges;
        $('#txtTotalBill').val(_total);
    }
});

$('#txtPaidBill').on('keyup change', function () {
    $('#txtRemainingBill').val('');
    if (this.value != '') {
        var _totalBill = parseInt($('#txtTotalBill').val());
        $('#txtRemainingBill').val(_totalBill - parseInt(this.value));
    }
});

$('#ddlPatient').change(function () {
    var doctorDropdown = $("#ddlDoctor");
    if (this.value != '') {
        $.ajax({
            url: '/PatientBillDetail/GetDoctorByPatientId',
            dataType: "JSON",
            type: "GET",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: { ID: this.value },
            async: false,
            success: function (result) {
                doctorDropdown.empty();
                if (result.length > 1) {
                    doctorDropdown.append("<option value=''>Select Doctor</option>");
                }
                $.each(result, function (i, data) {
                    doctorDropdown.append("<option value='" + data.value + "'>" + data.text + "</option>");
                });
                if (result.length == 1) {
                    _doctorCharges = GetPerDayCharges('doctor', result[0].value);
                    $('#txtNoOfDaysAdmitted').focus();
                }
            },
            error: function (xhr) {
                console.log(xhr.error);
            },
            complete: function () {
            }
        });
    }
    else {
        doctorDropdown.empty();
        doctorDropdown.append("<option value=''>Select Doctor</option>");
    }
})

