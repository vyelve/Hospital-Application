var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindPatientDetailsGrid(jsonData);
}

function BindPatientDetailsGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].hasInsurance = jsonData[i].hasInsurance === true ? "Yes" : "No";
        jsonData[i].hasAdmitted = jsonData[i].hasAdmitted === true ? "Yes" : "No";
    }
    $('#tblPatientDetails').bootstrapTable('destroy');
    $('#tblPatientDetails').bootstrapTable({
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
                field: 'patientFullName',
                title: 'Patient Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'gender',
                title: 'Gender',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'phoneNumber',
                title: 'Phone Number',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'dob',
                title: 'Date of Birth',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'age',
                title: 'Age',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '10%'
            },
            {
                field: 'emergencyContactPerson',
                title: 'Emergency Contact Person',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'emergencyContactNumber',
                title: 'Emergency Contact Number',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'doctorName',
                title: 'Doctor',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '40%'
            }]
    });
}

function getHeight() {
    return $(window).height();
}

function editRowFormatter(value, row, index) {
    return [
        '<a href="javascript:void(0);" class="editPatient fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeletePatient(\'' + row.patientID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function ReloadPage() {
    var Url = '/Patient/Index';
    location.href = Url;
}

function PatientDetailsModel() {
    this.patient = {
        patientID: $('#hdnPatientID').val(),
        patientNumber: $('#hdnPatientNumber').val(),
        firstName: $('#txtFirstName').val(),
        middleName: $('#txtMiddleName').val(),
        lastName: $('#txtLastName').val(),
        gender: $('#ddlGender option:selected').val(),
        phoneNumber: parseInt($('#txtPhoneNumber').val()),
        dob: $('#txtDOB').val(),
        age: $('#hdnAge').val(),
        maritalStatus: $('#ddlMaritalStatus option:selected').val(),
        emergencyContactPerson: $('#txtEmergencyContactPerson').val(),
        emergencyContactNumber: parseInt($('#txtEmergencyContactNumber').val()),
        address: $('#txtAddress').val(),
        medicalHistory: $('#txtMedicalHistory').val(),
        doctorId: $('#ddlDoctor option:selected').val(),
        doctorName: $('#ddlDoctor option:selected').text(),
        hasInsurance: $('#chkHasInsurance').prop('checked'),
        insuranceid: $('#txtInsuranceId').val(),
        hasAdmitted: $('#chkHasAdmitted').prop('checked'),
    },
        this.getPatientDetailsModel = function () {
            return this.patient;
        }
    this.setPatientDetailsModel = function (Data) {
        $('#hdnPatientID').val(Data.patientID);
        $('#hdnPatientNumber').val(Data.patientNumber);
        $('#txtFirstName').val(Data.firstName);
        $('#txtMiddleName').val(Data.middleName);
        $('#txtLastName').val(Data.lastName);
        $('#ddlGender').val(Data.gender.trim());
        $("#ddlGender option:contains(" + Data.gender.trim() + ")").attr('selected', true);
        $('#txtPhoneNumber').val(Data.phoneNumber);
        $('#txtDOB').val(Data.dob);
        $('#txtDOB').prop('disabled', true);
        $('#hdnAge').val(Data.age);
        $('#ddlMaritalStatus').val(Data.maritalStatus.trim());
        $("#ddlMaritalStatus option:contains(" + Data.maritalStatus.trim() + ")").attr('selected', true);

        $('#txtEmergencyContactPerson').val(Data.emergencyContactPerson);
        $('#txtEmergencyContactNumber').val(Data.emergencyContactNumber);
        $('#txtAddress').val(Data.address);
        $('#txtMedicalHistory').val(Data.medicalHistory);
        $('#ddlDoctor').val(Data.doctorId);
        Data.hasInsurance === "Yes" ? $('#chkHasInsurance').prop('checked', true).change() : $('#chkHasInsurance').prop('checked', false).change();
        $('#txtInsuranceId').val(Data.insuranceid);
        Data.hasAdmitted === "Yes" ? $('#chkHasAdmitted').prop('checked', true).change() : $('#chkHasAdmitted').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetPatientDetailsModel = function () {
        $('#hdnPatientID').val('0');
        $('#hdnPatientNumber').val('');
        $('#txtFirstName').val('');
        $('#txtMiddleName').val('');
        $('#txtLastName').val('');
        $('#ddlGender option:eq(0)').attr('selected', 'selected');
        $('#ddlGender').prop('SelectedIndex', 0);
        $('#ddlGender').val('');
        $('#ddlMaritalStatus option:eq(0)').attr('selected', 'selected');
        $('#ddlMaritalStatus').prop('SelectedIndex', 0);
        $('#ddlMaritalStatus').val('');
        $('#txtPhoneNumber').val('');
        $('#txtDOB').val('');
        $('#txtDOB').prop('disabled', false);
        $('#hdnAge').val('');
        $('#txtEmergencyContactPerson').val('');
        $('#txtEmergencyContactNumber').val('');
        $('#txtAddress').val('');
        $('#txtMedicalHistory').val('');
        $('#ddlDoctor option:eq(0)').attr('selected', 'selected');
        $('#ddlDoctor').prop('SelectedIndex', 0);
        $('#chkHasInsurance').prop('checked', true).change();
        $('#txtInsuranceId').val('');
        $('#chkHasAdmitted').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#patientform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SavePatientDetails();
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
        var _patientModel = new PatientDetailsModel();
        _patientModel.resetPatientDetailsModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

var SavePatientDetails = function () {
    var _patientModel = new PatientDetailsModel();
    var _model = _patientModel.getPatientDetailsModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);
    console.log(jsonData);

    $.ajax({
        url: _command === "Submit" ? '/Patient/CreatePatientDetails' : '/Patient/EditPatientDetails',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { patientModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _patientModel.resetPatientDetailsModel();
            console.log(result.tblPatientDetails);
            BindPatientDetailsGrid(result.tblPatientDetails);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeletePatient(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Patient/DeletePatient',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "PatientID": ID },
                    success: function (res) {
                        SetAlert('success', 'Record Deleted Successfully.');
                        BindPatientDetailsGrid(res.tblPatientDetails);
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

//function CalculatePatientAge() {
//    $('#txtAge').val('');
//    var _dateofBirth = $('#txtDOB');
//    if (_dateofBirth != '') {

//        var splitDate = _dateofBirth.split('-');
//        var DOB = splitDate[2] + '-' + splitDate[1] + '-' + splitDate[0]; // YYYY-MM-DD

//        var today = new Date();
//        var age = Math.floor((today - DOB) / (365.25 * 24 * 60 * 60 * 1000));

//        $('#txtAge').val(age);
//    }
//}