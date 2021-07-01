var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;
var _totalBillAmount = 0;

if (jsonData !== null) {
    BindPaymentDetailsGrid(jsonData);
}

function BindPaymentDetailsGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isDischarged = jsonData[i].isDischarged === true ? "Yes" : "No";
    }
    $('#tblPaymentDetails').bootstrapTable('destroy');
    $('#tblPaymentDetails').bootstrapTable({
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
                title: '',
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
                field: 'amount',
                title: 'Amount',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'paymentType',
                title: 'Payment Type',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'bankName',
                title: 'Bank Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'isDischarged',
                title: 'Discharged',
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
    return [
        '<a href="javascript:void(0);" class="fas fa-exclamation-circle" style="pointer-events: none;" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function ReloadPage() {
    var Url = '/PaymentDetail/Index';
    location.href = Url;
}

function PaymentDetailModel() {
    this.payment = {
        paymentId: $('#hdnPaymentId').val(),
        billId: $('#ddlBillNumber option:selected').val(),
        billNumber: $('#ddlBillNumber option:selected').text(),
        patientId: $('#ddlPatient option:selected').val(),
        patientName: $('#ddlPatient option:selected').text(),
        totalBill: $('#txtTotalBill').val(),
        amount: $('#txtAmount').val(),
        paymentType: $('#ddlPaymentType option:selected').val(),
        bankName: $('#txtBankName').val(),
    },
        this.getPaymentDetailModel = function () {
            return this.payment;
        }
    this.setPaymentDetailModel = function (Data) {
        $('#hdnPaymentId').val(Data.paymentId);
        $('#ddlBillNumber').val(Data.billId);
        $('#ddlPatient').val(Data.patientId);
        $('#txtTotalBill').val(Data.totalBill);
        $('#txtAmount').val(Data.amount);
        $('#ddlPaymentType').val(Data.paymentType.trim());
        $("#ddlPaymentType option:contains(" + Data.paymentType.trim() + ")").attr('selected', true);
        $('#txtBankName').val(Data.bankName);
        $("#btnSubmit").val('Update');
    }
    this.resetPaymentDetailModel = function () {
        $('#hdnPaymentId').val('0');
        $('#ddlBillNumber option:eq(0)').attr('selected', 'selected');
        $('#ddlBillNumber').prop('SelectedIndex', 0);
        $('#ddlPatient option:eq(0)').attr('selected', 'selected');
        $('#ddlPatient').prop('SelectedIndex', 0);
        $('#txtTotalBill').val('');
        $('#txtAmount').val('');
        $('#ddlPaymentType option:eq(0)').attr('selected', 'selected');
        $('#ddlPaymentType').prop('SelectedIndex', 0);
        $('#ddlPaymentType').val('');
        $('#txtBankName').val('');
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#paymentform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SavePaymentDetails();
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
        var _paymentDetailModel = new PaymentDetailModel();
        _paymentDetailModel.resetPaymentDetailModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

var SavePaymentDetails = function () {
    var _paymentDetailModel = new PaymentDetailModel();
    var _model = _paymentDetailModel.getPaymentDetailModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);
    console.log(jsonData);

    $.ajax({
        url: _command === "Submit" ? '/PaymentDetail/CreatePaymentDetails' : '/PaymentDetail/EditPaymentDetails',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { paymentModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _paymentDetailModel.resetPaymentDetailModel();
            console.log(result.tblPayment);
            BindPaymentDetailsGrid(result.tblPayment);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

$('#ddlBillNumber').change(function () {
    $('#txtTotalBill').val('');
    GetPatientsNameByBillId(this.value);
    _totalBillAmount = GetPatientsTotalBillByBillId(this.value);
    $('#txtTotalBill').val(_totalBillAmount);
});

function GetPatientsTotalBillByBillId(_billId) {
    var totalbill = 0;
    if (_billId != '') {
        $.ajax({
            url: '/PaymentDetail/GetPatientTotalBill',
            dataType: "JSON",
            type: "GET",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: { BillId: _billId },
            async: false,
            success: function (result) {
                if (result != '') {
                    totalbill = result;
                }
            },
            error: function (e) {
                SetAlert('error', 'Error Occured While Processing Data.');
            },
            complete: function () {
            }
        });
    }
    return totalbill;
}

function GetPatientsNameByBillId(_billId) {
    var patientDropdown = $("#ddlPatient");
    if (_billId != '') {
        $.ajax({
            url: '/PaymentDetail/GetPatientNameByBillId',
            dataType: "JSON",
            type: "GET",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: { BillId: _billId },
            async: false,
            success: function (result) {
                patientDropdown.empty();
                if (result.length > 1) {
                    patientDropdown.append("<option value=''>Select Patient Name</option>");
                }
                $.each(result, function (i, data) {
                    patientDropdown.append("<option value='" + data.value + "'>" + data.text + "</option>");
                });
            },
            error: function (xhr) {
                console.log(xhr.error);
            },
            complete: function () {
            }
        });
    }
    else {
        patientDropdown.empty();
        patientDropdown.append("<option value=''>Select Patient Name</option>");
    }
}

$('#ddlPaymentType').change(function () {
    $('#txtBankName').prop('disabled', false);
    var type = $("#ddlPaymentType :selected").text();
    if (type.toLowerCase().trim() == 'cash') {
        $('#txtBankName').prop('disabled', true);
    }
});