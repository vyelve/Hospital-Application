var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindUserGrid(jsonData);
}

function BindUserGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }
    $('#tblUser').bootstrapTable('destroy');
    $('#tblUser').bootstrapTable({
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
                field: 'userShortName',
                title: 'Short Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'firstName',
                title: 'First Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'lastName',
                title: 'Last Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'emailId',
                title: 'Email Id',
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
                field: 'designationName',
                title: 'Designation',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '40%'
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
                field: 'phoneNumber',
                title: 'Phone Number',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'doj',
                title: 'DOJ',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'isActive',
                title: 'Active',
                align: 'center',
                valign: 'bottom',
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
        '<a href="javascript:void(0);" class="editUser fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteUser(\'' + row.userID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function UserModel() {
    this.user = {
        userID: $('#hdnUserID').val(),
        firstName: $('#txtFirstName').val(),
        lastName: $('#txtLastName').val(),
        userShortName: $('#txtUserShortName').val(),
        emailId: $('#txtEmailId').val(),
        gender: $('#ddlGender option:selected').val(),
        designationId: $('#ddlDesignation option:selected').val(),
        designationName: $('#ddlDesignation option:selected').text(),
        departmentID: $('#ddlDepartment option:selected').val(),
        departmentName: $('#ddlDepartment option:selected').text(),
        phoneNumber: parseInt($('#txtPhoneNumber').val()),
        password: $('#txtPassword').val(),
        confirmPassword: $('#txtConfirmPassword').val(),
        DOJ: $('#txtDOJ').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
    this.getUserModel = function () {
            return this.user;
        }
    this.setUserModel = function (Data) {
        $('#hdnUserID').val(Data.userID);
        $('#txtFirstName').val(Data.firstName);
        $('#txtLastName').val(Data.lastName);

        $('#txtUserShortName').prop('disabled', false);
        $('#txtUserShortName').val(Data.userShortName);
        $('#txtUserShortName').prop('disabled', true);

        $('#txtEmailId').val(Data.emailId);
        $('#ddlGender').val(Data.gender.trim());

        $("#ddlGender option:contains(" + Data.gender.trim() + ")").attr('selected', true);

        $('#ddlDesignation').val(Data.designationId);
        $('#ddlDepartment').val(Data.departmentID);
        $('#txtPhoneNumber').val(Data.phoneNumber);
        $('#txtPassword').val(Data.password);
        $('#txtConfirmPassword').val(Data.confirmPassword);

        $('#txtPassword').prop('disabled', true);
        $('#txtConfirmPassword').prop('disabled', true);

        $('#txtDOJ').val(Data.doj);
        $('#txtDOJ').prop('disabled', true);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetUserModel = function () {
        $('#hdnUserID').val('0');
        $('#txtFirstName').val('');
        $('#txtLastName').val('');
        $('#txtUserShortName').val('');
        $('#txtEmailId').val('');

        $('#ddlGender option:eq(0)').attr('selected', 'selected');
        $('#ddlGender').prop('SelectedIndex', 0);
        $('#ddlGender').val('');

        $('#ddlDesignation option:eq(0)').attr('selected', 'selected');
        $('#ddlDesignation').prop('SelectedIndex', 0);
        $('#ddlDesignation').val('');

        $('#ddlDepartment option:eq(0)').attr('selected', 'selected');
        $('#ddlDepartment').prop('SelectedIndex', 0);
        $('#ddlDepartment').val('');

        $('#txtPhoneNumber').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('#txtDOJ').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');

        $('#txtDOJ').prop('disabled', false);
        $('#txtPassword').prop('disabled', false);
        $('#txtConfirmPassword').prop('disabled', false);
    }
}

$(function () {

    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#userform").valid()) {
            submit = false;
            SetAlertDiv("myAlert", true, 'Warning! ', 'Validation failed.');
        }
        if ($Submitbtn.val() !== "Submit") {
            var _userModel = new UserModel();
            var ID = _userModel.getUserModel().userID;

            if (!(ValidatedNurseUserActive(ID))) {
                submit = false;
            }
        }
        if (submit) {
            try {
                SaveUserDetails();
                window.existingModel = null;
                reloadPage();
            }
            catch (err) {
                if (arguments !== null && arguments.callee !== null && arguments.callee.trace)
                    logError(err, arguments.callee.trace());
                SetAlertDiv("myAlert", true, 'Warning! ', 'Error Occured While Processing Data.');
            }
        }
    });

    $Cancelbtn.bind('click', function (e) {
        e.preventDefault();
        var _userModel = new UserModel();
        _userModel.resetUserModel();
        Window.existingModel = null;
        reloadPage();

    });
});

var SaveUserDetails = function () {
    var _userModel = new UserModel();
    var _model = _userModel.getUserModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);
    console.log(jsonData);

    $.ajax({
        url: _command === "Submit" ? '/User/CreateUser' : '/User/EditUser',
        dataType: "JSON",
        type: "POST",
        cache: false,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { userModel: jsonData },
        success: function (result) {
            if (_command === "Submit") {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been added.');
            }
            else {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been updated.');
            }
            _userModel.resetUserModel();
            console.log(result.tblUsers);
            BindUserGrid(result.tblUsers);
        },
        error: function (e) {
            SetAlertDiv("myAlert", true, 'Warning! ', ' Error Occured While Processing Data.');
        },
        complete: function () {
            
        }
    });
}

function ValidatedNurseUserActive(ID) {   
    var result = false;
    if (ID !== "") {
        $.ajax({
            url: '/User/ValidatedNurseUserActive',
            type: "GET",
            dataType: "JSON",
            async: false,
            data: { "UserId": ID },
            success: function (res) {
                if (res.message.toLowerCase() === "user is mapped") {
                    SetAlertDiv("myAlert", true, 'Warning! ', 'User is Mapped cannot Modify.');
                    result = false;
                }
                else {
                    result = true;
                }
            },
            error: function (e) {
                SetAlertDiv("myAlert", true, 'Warning! ', 'Error Occured While Processing Data.');
            },
            complete: function () {
               
            }
        });
    }
    return result;
}

function reloadPage() {
    var Url = '/User/Index';
    location.href = Url;
}

function DeleteUser(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (!(ValidatedNurseUserActive(ID))) {
            result = false;
        }
        if (result) {

            if (ID !== "") {
                $.ajax({
                    url: '/User/DeleteUser',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "UserId": ID },
                    success: function (res) {
                        SetAlertDiv("myAlert", false, 'Success! ', ' Record Deleted Successfully');
                        BindUserGrid(res.tblUsers);
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

$('#txtLastName').keyup(function () {
    if ($('#txtFirstName').val() !== '' && $('#txtLastName').val() !== '') {
        var first = $('#txtFirstName').val().slice(0, 2);
        var second = $('#txtLastName').val().slice(0, $('#txtLastName').val().length);
        var combine = first + second
        console.log(combine);
        $('#txtUserShortName').val(combine.toLowerCase());
    }
});
