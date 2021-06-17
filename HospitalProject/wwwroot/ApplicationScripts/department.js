var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindDepartmentGrid(jsonData);
}

function BindDepartmentGrid(jsonData) {

    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblDepartment').bootstrapTable('destroy');
    $('#tblDepartment').bootstrapTable({
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
                field: 'departmentName',
                title: 'Department',
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

        '<a href="javascript:void(0);" class="editdept fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteDepartment(\'' + row.deptId + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function DepartmentModel() {
    this.department = {
        deptId: $('#hdnDeptId').val(),
        departmentName: $('#txtDepartmentName').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
        this.getDepartmentModel = function () {
            return this.department;
        }
    this.setDepartmentModel = function (Data) {
        $('#hdnDeptId').val(Data.deptId);
        $('#txtDepartmentName').val(Data.departmentName);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetDepartmentModel = function () {
        $("#hdnDeptId").val('0');
        $('#txtDepartmentName').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {

    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#departmentform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveDepartmentData();
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
        var _departmentModel = new DepartmentModel();
        _departmentModel.resetDepartmentModel();
        Window.existingModel = null;

        var Url = '/Department/Index';
        location.href = Url;
    });
});

var SaveDepartmentData = function () {
    var _departmentModel = new DepartmentModel();
    var _model = _departmentModel.getDepartmentModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/Department/CreateDepartment' : '/Department/EditDepartment',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { departmentModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been added.');
            }
            else {
                SetAlertDiv("myAlert", false, 'Success! ', ' Record has been updated.');
            }
            _departmentModel.resetDepartmentModel();
            BindDepartmentGrid(result.tblDepartment);
        },
        error: function (e) {
            SetAlertDiv("myAlert", true, 'Warning! ', ' Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteDepartment(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/department/DeleteDepartment',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "DeptId": ID },
                    success: function (res) {
                        SetAlertDiv("myAlert", false, 'Success! ', ' Record Deleted Successfully');
                        BindDepartmentGrid(res.tblDepartment);
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