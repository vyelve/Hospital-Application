var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindRoomGrid(jsonData);
}

function BindRoomGrid(jsonData) {

    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblRoom').bootstrapTable('destroy');
    $('#tblRoom').bootstrapTable({
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
                field: 'roomType',
                title: 'Room Name',
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
        '<a href="javascript:void(0);" class="editRoom fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteRoom(\'' + row.roomId + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function ReloadPage() {
    var Url = '/Room/Index';
    location.href = Url;
}

function RoomDetailsModel() {
    this.room = {
        roomId: $('#hdnRoomId').val(),
        roomType: $('#txtRoomType').val(),
        per_Day_Charges: $('#txtPerDayCharges').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
    this.getRoomDetailsModel = function () {
        return this.room;
    }
    this.setRoomDetailsModel = function (Data) {
        $('#hdnRoomId').val(Data.roomId);
        $('#txtRoomType').val(Data.roomType);
        $('#txtPerDayCharges').val(Data.per_Day_Charges);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetRoomDetailsModel = function () {
        $('#hdnRoomId').val('0');
        $('#txtRoomType').val('');
        $('#txtPerDayCharges').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#roomform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveRoomData();
                window.existingModel = null;
                setTimeout(function () { ReloadPage(); }, 4000);
            }
            catch (err) {
                if (arguments !== null && arguments.callee !== null && arguments.callee.trace)
                    logError(err, arguments.callee.trace());
            }
        else {
            //SetAlertDiv("myAlert", true, 'Warning! ', 'Validation failed.');
            SetAlert('error', 'Validation failed.');
        }
    });

    $Cancelbtn.bind('click', function (e) {
        e.preventDefault();
        var _roomModel = new RoomDetailsModel();
        _roomModel.resetRoomDetailsModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

var SaveRoomData = function () {
    var _roomModel = new RoomDetailsModel();
    var _model = _roomModel.getRoomDetailsModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/Room/CreateRoom' : '/Room/EditRoom',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { roomModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                //SetAlertDiv("myAlert", false, 'Success! ', ' Record has been added.');
                SetAlert('success', 'Record has been added.');
            }
            else {
                //SetAlertDiv("myAlert", false, 'Success! ', ' Record has been updated.');
                SetAlert('success', 'Record has been updated.');
            }
            _roomModel.resetRoomDetailsModel();
            BindRoomGrid(result.tblRoom);
        },
        error: function (e) {
            //SetAlertDiv("myAlert", true, 'Warning! ', ' Error Occured While Processing Data.');
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteRoom(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Room/DeleteRoom',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "ID": ID },
                    async: false,
                    success: function (res) {
                        //SetAlertDiv("myAlert", false, 'Success! ', ' Record Deleted Successfully');
                        SetAlert('success', 'Record Deleted Successfully.');
                        BindRoomGrid(res.tblRoom); 
                        setTimeout(function () { ReloadPage(); }, 4000);
                    },
                    error: function (e) {
                        //SetAlertDiv("myAlert", true, 'Warning! ', 'Error Occured While Processing Data.');
                        SetAlert('error', 'Error Occured While Processing Data.');
                    },
                    complete: function () {
                    }
                });
            }
        }
    });
}